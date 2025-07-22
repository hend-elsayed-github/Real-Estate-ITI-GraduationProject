using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;
using Stripe;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PaymentController : ControllerBase
    {
        IRentRepositories _rentRepository;
        IPaymentRepository _paymentRepository;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IRentRepositories rentRepository, IPaymentRepository paymentRepository, ILogger<PaymentController> logger)
        {
            _rentRepository = rentRepository;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }
        [Authorize(Roles = "Owner")]
        [HttpGet("Stripe/Onboarding")]
        //public async Task<IActionResult> CreateStripeAccountLink()
        //{
        //    // جيب بيانات المستخدم الحالي
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //        return Unauthorized();

        //    // لو المستخدم مالوش Stripe Account، نعمل واحد
        //    if (string.IsNullOrEmpty(user.StripeAccountId))
        //    {
        //        var accountService = new AccountService();
        //        var account = await accountService.CreateAsync(new AccountCreateOptions
        //        {
        //            Type = "standard"
        //        });

        //        user.StripeAccountId = account.Id;
        //        await _userManager.UpdateAsync(user);
        //    }

        //    // نعمل رابط Onboarding
        //    var accountLinkService = new AccountLinkService();
        //    var accountLink = await accountLinkService.CreateAsync(new AccountLinkCreateOptions
        //    {
        //        Account = user.StripeAccountId!,
        //        RefreshUrl = "https://localhost:4200/reauth",  // لو فشل
        //        ReturnUrl = "https://localhost:4200/complete", // لو نجح
        //        Type = "account_onboarding"
        //    });

        //    // ارجعي الرابط للفرونت إند (بدل ما تعملي Redirect ممكن ترجعيه JSON)
        //    return Ok(new { url = accountLink.Url });
        //}


        [Authorize(Roles="renter")]
        [HttpPost("Payment")]
        public async Task<IActionResult> CreatePaymentIntent( int rentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var rent = await _rentRepository.GetRentByIdAsync(rentId, userId);
            if (rent == null || rent.IsPaid)
                return BadRequest("Invalid or already paid rent");

            var owner = rent.unit.owner; 
            if (string.IsNullOrEmpty(owner.StripeAccountId))
                return BadRequest("Owner doesn't have a Stripe account");

            var paymentIntentService = new PaymentIntentService();

            var intent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = (long)(rent.Rentvalue * 100),
                Currency = "usd", 
                PaymentMethodTypes = new List<string> { "card" },
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




        [HttpPost("webhooks/stripe")]
    
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"], "whsec_v1pMEnScasI5tcS8nvEmscPISjUEVSIG"
                );

                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    if (paymentIntent?.Metadata != null && paymentIntent.Metadata.ContainsKey("RentId")                   
                             && paymentIntent.Metadata.ContainsKey("RenterId"))
                    {
                        var rentId = int.Parse(paymentIntent.Metadata["RentId"]);
                        var renterId = paymentIntent.Metadata["RenterId"];

                        var paymentMethodId = paymentIntent.PaymentMethodId; 
                        var service = new PaymentMethodService();

                        var paymentMethod = await service.GetAsync(paymentMethodId);
                        var type = paymentMethod.Type;

                        var StripePaymentMethodId= paymentMethod.Id;
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

    }
}
