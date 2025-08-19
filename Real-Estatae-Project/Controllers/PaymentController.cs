using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Real_Estatae_Project.DTO.payment;
using Real_Estatae_Project.Hubs;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;


namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly IRentRepositories _rentRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly  IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<PaymentController> _logger;
        private readonly IConfiguration _configuration;


        public PaymentController(IRentRepositories rentRepository, IPaymentRepository paymentRepository, IUserRepository userRepository, INotificationRepository NotificationRepository,
            IHubContext<NotificationHub> hubContext, ILogger<PaymentController> logger,IConfiguration configuration)

        {
            _rentRepository = rentRepository;
            _paymentRepository = paymentRepository;
            _logger = logger;
            _userRepository = userRepository;
            _notificationRepository = NotificationRepository;
            _hubContext = hubContext;
            _configuration = configuration;
        }
        #region owner account

        [Authorize(Roles = "Owner")]
        [HttpGet("Stripe/Onboarding")]
        public async Task<IActionResult> CreateStripeAccountLink()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            // 2. Create account if not exists
            if (string.IsNullOrEmpty(user.StripeAccountId))
            {
                var accountService = new AccountService();
                var account = await accountService.CreateAsync(new AccountCreateOptions
                {
                    Type = "standard"
                });

                user.StripeAccountId = account.Id;
                await _userRepository.Update(user);
            }

            // 3. Check if account is already completed
            var accountCheck = new AccountService();
            var accountStatus = await accountCheck.GetAsync(user.StripeAccountId);

            if (accountStatus.ChargesEnabled && accountStatus.PayoutsEnabled)
            {
                return Ok("Your Stripe account is already activated");
            }

            // 4. Create onboarding link
            var accountLinkService = new AccountLinkService();
            var accountLink = await accountLinkService.CreateAsync(new AccountLinkCreateOptions
            {
                Account = user.StripeAccountId,
                RefreshUrl = "https://localhost:4200/reauth",
                ReturnUrl = "https://localhost:4200/complete",
                Type = "account_onboarding"
            });

            return Ok(accountLink.Url);
        }


        #endregion
        #region Payment Intent
        [Authorize(Roles="Renter")]
        [HttpPost("PaymentIntent")]
        public async Task<IActionResult> CreatePaymentIntent( int rentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var rent = await _rentRepository.GetRentByIdAsync(rentId, userId);

            if (rent == null)
                return BadRequest("Invalid  rent");

            if( rent.IsPaid)

                return BadRequest("already paid rent");

            var owner = rent.unit.owner; 
            if (string.IsNullOrEmpty(owner.StripeAccountId))
                return BadRequest("Owner doesn't have a Stripe account");

            var paymentIntentService = new PaymentIntentService();     
            var intent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = (long)(rent.Rentvalue * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
                ApplicationFeeAmount = (long)((decimal)rent.Rentvalue * 0.05m * 100), 
                TransferData = new PaymentIntentTransferDataOptions
                {
                    Destination = owner.StripeAccountId
                },
                Metadata = new Dictionary<string, string>
                    {
                        { "RentId", rent.id.ToString() },
                        { "RenterId", userId }
                    }
                    });

            return Ok(new { clientSecret = intent.ClientSecret });
        }

        #endregion

        #region session
        [HttpPost("CreateCheckoutSession")]
        [Authorize(Roles = "Renter")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] PaymentDTO rentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var rent = await _rentRepository.GetRentByIdAsync(rentId.RentId, userId);

            if (rent == null)
                return BadRequest("Invalid  rent");

            if (rent.IsPaid)

                return BadRequest("already paid rent");

            var owner = rent.unit.owner;
            if (string.IsNullOrEmpty(owner.StripeAccountId))
                return BadRequest("Owner doesn't have a Stripe account");

            var domain = _configuration["JWT:AudienceIP"];
            var adminFee = (long)((decimal)rent.Rentvalue * 0.05m * 100);
            var landlordAccountId = owner.StripeAccountId;
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)(rent.Rentvalue * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = $"Rent for {rent.dueDate:MMMM yyyy}"
                    }
                },
                Quantity = 1
            }
        },
                Mode = "payment",
                SuccessUrl = $"{domain}/renterTabs",
                CancelUrl = $"{domain}/renterTabs",

                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    ApplicationFeeAmount = adminFee, 
                    TransferData = new SessionPaymentIntentDataTransferDataOptions
                    {
                        Destination = landlordAccountId 
                    },
                    Metadata = new Dictionary<string, string>
                        {
                            { "RentId", rent.id.ToString() },
                            { "RenterId", userId }
                        }
                }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);
  //inset in db

            await _rentRepository.UpdateRentAsync(rentId.RentId);

            var payment = new Payment
            {
                Amount = rent.unit.price,
                RentId = rentId.RentId,
                UserId = rent.unit.renterId,
                StripePaymentIntentId = "paymentIntent.Id",
                paymentType = "card",
                CardBrand = "cardBrand",
                CardLast4 = "CardLast4",
                StripePaymentMethodId = "StripePaymentMethodId"

            };

            var result = await _paymentRepository.createPayment(payment);
            //INotification
            var user = await _userRepository.FindByIdAsync(userId);
            string userName = user.firstName + " " + user.lastName;
            var ownerid = rent.unit.ownerId;

            string notificationMessage = $"{userName} Paying rent for {rent.dueDate}";         
                var notification = new Notification
                {
                    userId = ownerid,
                    sender = userName,
                    message = notificationMessage,

                };
                await _notificationRepository.AddAsync(notification);

                // signlR
                await _hubContext.Clients.User(ownerid).SendAsync("ReceiveNotification", new
                {
                    message = notificationMessage,
                    sender = userName,
                    createdAt = DateTime.UtcNow
                });
            return Ok(new { url = session.Url });
        }
        #endregion

        #region webhook

        [HttpPost("webhooks/stripe")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            _logger.LogInformation("Webhook received: " + json);


            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
            json,
            Request.Headers["Stripe-Signature"],
            _configuration["Stripe:WebhookSecret"]
            );
                
              //  if (stripeEvent.Type == "payment_intent.succeeded" )
                if ( stripeEvent.Type == "checkout.session.completed")
                {
                  //  var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    var session = stripeEvent.Data.Object as Session;

                    var paymentIntentId = session.PaymentIntentId;
                    var paymentIntentService = new PaymentIntentService();
                    var paymentIntent = await paymentIntentService.GetAsync(paymentIntentId);

                    if (paymentIntent?.Metadata != null && paymentIntent.Metadata.ContainsKey("RentId")                   
                             && paymentIntent.Metadata.ContainsKey("RenterId"))
                    {
                        var rentId = int.Parse(paymentIntent.Metadata["RentId"]);
                        var renterId = paymentIntent.Metadata["RenterId"];

                        var paymentMethodId = paymentIntent.PaymentMethodId; 
                        var service = new PaymentMethodService();

                        var paymentMethod = await service.GetAsync(paymentMethodId);
                        var type = paymentMethod.Type;

                        var StripePaymentMethodId = paymentMethod.Id;
                        string cardBrand = null;
                        string last4 = null;


                        if (paymentMethod.Type == "card" && paymentMethod.Card != null)
                        {
                            cardBrand = paymentMethod.Card.Brand;
                            last4 = paymentMethod.Card.Last4;
                        }
                     
                        await _rentRepository.UpdateRentAsync(rentId);

                        var payment = new Payment
                        {
                            Amount = paymentIntent.Amount / 100.0,
                            RentId = rentId,
                            UserId = renterId,
                            StripePaymentIntentId = paymentIntent.Id,
                            paymentType = type,
                            CardBrand = cardBrand,
                            CardLast4=last4,
                            StripePaymentMethodId = StripePaymentMethodId

                        };

                       var result= await _paymentRepository.createPayment(payment);
              
                        return Ok();

                    }
                    else
                    {
                        _logger.LogWarning("Missing metadata in paymentIntent");
                        return BadRequest("Missing metadata.");
                    }
                }

                return Ok();
            }

            catch (StripeException ex)
            {
                _logger.LogWarning(ex, "Stripe webhook signature or Stripe-related error.");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stripe webhook general error.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        #endregion

        #region teast endpoint

      
        [Authorize(Roles = "Renter")]
        [HttpPost("Payment")]
        public async Task<IActionResult> Payment([FromBody] PaymentDTO PaymentDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            int rentId = PaymentDTO.RentId;
            var rent = await _rentRepository.GetRentByIdAsync(rentId, userId);

            if (rent == null)
                return BadRequest("Invalid  rent");

            if (rent.IsPaid)
                return BadRequest("already paid rent");


            var owner = rent.unit.owner;
            if (string.IsNullOrEmpty(owner.StripeAccountId))
                return BadRequest("Owner doesn't have a Stripe account");

            var paymentIntentService = new PaymentIntentService();

            var intent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = (long)(rent.Rentvalue * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
                ApplicationFeeAmount = (long)((decimal)rent.Rentvalue * 0.05m * 100), 
                TransferData = new PaymentIntentTransferDataOptions
                {
                    Destination = owner.StripeAccountId
                },
                Metadata = new Dictionary<string, string>
        {
            { "RentId", rent.id.ToString() },
              { "RenterId", userId }
        }
            });


            //inset in db

            await _rentRepository.UpdateRentAsync(rentId);

            var payment = new Payment
            {
                Amount = rent.unit.price,
                RentId = rentId,
                UserId = rent.unit.renterId,
                StripePaymentIntentId = "paymentIntent.Id",
                paymentType = "card",
                CardBrand = "cardBrand",
                CardLast4 = "CardLast4",
                StripePaymentMethodId = "StripePaymentMethodId"

            };

            var result = await _paymentRepository.createPayment(payment);
            //INotification
            var user = await _userRepository.FindByIdAsync(userId);
            string userName = user.firstName + " " + user.lastName;
            var ownerid = rent.unit.ownerId;

            string notificationMessage = $"{userName} Paying rent for {rent.dueDate}";         
                var notification = new Notification
                {
                    userId = ownerid,
                    sender = userName,
                    message = notificationMessage,

                };
                await _notificationRepository.AddAsync(notification);

                // signlR
                await _hubContext.Clients.User(ownerid).SendAsync("ReceiveNotification", new
                {
                    message = notificationMessage,
                    sender = userName,
                    createdAt = DateTime.UtcNow
                });
                return Ok(new { clientSecret = intent.ClientSecret });
        }
        #endregion
    }
}
