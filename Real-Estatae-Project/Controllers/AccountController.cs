using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Real_Estatae_Project.DTO.Auth;
using Real_Estatae_Project.DTO.Password;
using Real_Estatae_Project.DTO.User;
using Real_Estatae_Project.Repositories;
using Real_Estatae_Project.Services;
using Real_Estate_Project.Models;
using Stripe;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private ICommunityRepository communityRepo;
        private IConfiguration Config { get; } 
        private ICloudinaryRepository cloudinaryRepository;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration Config, IEmailService emailService, ICommunityRepository _communityRepo, ICloudinaryRepository cloudinaryRepository)
        {
            this.userManager = userManager;
            this.Config = Config;
            this.communityRepo = _communityRepo;
            this.cloudinaryRepository = cloudinaryRepository;
            this._emailService = emailService;


        }



        #region Registeration
        [HttpPost("Register")] // api/Account/Register
        [RequestSizeLimit(104_857_600)] // 100 MB
        [RequestFormLimits(MultipartBodyLengthLimit = 104_857_600)]
        public async Task<IActionResult> Register([FromForm] RegisterDTO userFromRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApplicationUser user = new ApplicationUser
            {
                UserName = userFromRequest.userName,
                Email = userFromRequest.email,
                firstName = userFromRequest.firstName,
                lastName = userFromRequest.lastName,
                PhoneNumber = userFromRequest.phone
            };

            IdentityResult result = await userManager.CreateAsync(user, userFromRequest.password);

          
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("Errors", error.Description);

                return BadRequest(ModelState);
            }

            if (userFromRequest.imageFile != null && userFromRequest.imageFile.Length > 0)
            {
                //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userFromRequest.imageFile.FileName);
                //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await userFromRequest.imageFile.CopyToAsync(stream);
                //}

                //Upload to Cloudinary
                var fileName = await cloudinaryRepository.UploadImageAsync(userFromRequest.imageFile);
                user.image = fileName;
                await userManager.UpdateAsync(user); 
            }

            string role = userFromRequest.role ?? "Visitor";
            if (role != "Owner" && role != "Renter")
                role = "Visitor";

            await userManager.AddToRoleAsync(user, role);

            if (role == "Owner")
            {

                var accountService = new AccountService();
                var stripeAccount = await accountService.CreateAsync(new AccountCreateOptions
                {
                    Type = "standard",
                    Email = user.Email,
                    BusinessType = "individual"
                });


                //user.StripeAccountId = stripeAccount.Id;
                user.StripeAccountId = "acct_1RnAYNIgZmVK93tC";

                await userManager.UpdateAsync(user);

                // Create onboarding link
                var accountLinkService = new AccountLinkService();
                var accountLink = await accountLinkService.CreateAsync(new AccountLinkCreateOptions
                {
                    Account = stripeAccount.Id,
                    RefreshUrl = Config["JWT:AudienceIP"], // where to send user if onboarding fails
                    ReturnUrl = Config["JWT:AudienceIP"]+"/login", // where to send user after success
                    Type = "account_onboarding"
                });
                Community newComm = new Community
                {
                    name = "new community",
                    ownerId = user.Id 
                };

                communityRepo.Create(newComm);
                communityRepo.Save();

                return Ok(new
                {
                    message = "Owner registered successfully",
                    onboardingUrl = accountLink.Url 
                });
            
            }

            
            return Ok(new {message= "User registered successfully" });
        }

        #endregion


        #region login

        [HttpPost("Login")]  //api/Account/Login

        public async Task<IActionResult> Login(LoginDTO userFromRequest)

        {
            if (ModelState.IsValid)
            {
                //Check user in DB? 
                
                ApplicationUser userFromDB = await userManager.FindByNameAsync(userFromRequest?.userName);

                if (userFromDB == null )
                {
                    userFromDB = await userManager.Users.FirstOrDefaultAsync(u => u.Email == userFromRequest.email);

                }

                if (userFromDB != null && userFromDB.isActive==true)
                {
                    //Check Password 
                    bool Found = await userManager.CheckPasswordAsync(userFromDB, userFromRequest.password);

                    if (Found == true)
                    {
                        //Generate Token 

                        //First ==> Desgin Token


                        //Customize Claims List
                        List<Claim> UserClaims = new List<Claim>();

                        //Create uniuqe Token Generated ID
                        UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        //Get ID
                        UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDB.Id));

                        //Get UserName
                        UserClaims.Add(new Claim(ClaimTypes.Name, userFromDB.UserName));

                        var UserRoles = await userManager.GetRolesAsync(userFromDB);

                        foreach (var roleName in UserRoles)
                        {
                            UserClaims.Add(new Claim(ClaimTypes.Role, roleName));
                        }

                        //Create SiningCredentials 
                        //1- Key ==> Symmetric Security Key
                        SymmetricSecurityKey SignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["JWT:SecretKey"]));

                        SigningCredentials signingCred = new SigningCredentials(SignKey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken myToken = new JwtSecurityToken(

                            issuer: Config["JWT:IssuerIP"], //who made Token 
                            audience: Config["JWT:AudienceIP"], //Who will use Token => consumer 
                            expires: DateTime.Now.AddHours(1), //Token Expiration Time 

                            claims: UserClaims,

                            signingCredentials: signingCred

                            );

                        //Generate Token 
                        //Send in Response 
                        return Ok(new
                        {
                            
                            token = new JwtSecurityTokenHandler().WriteToken(myToken),// gereate & Make it string
                            expiration = DateTime.Now.AddHours(72), // OR myTokn.expires
                            role= UserRoles.FirstOrDefault(),
                            userName=userFromDB.UserName,
                            image=userFromDB.image 

                            
                        });
                    }

                }
                ModelState.AddModelError("Error", "Invalid UserName OR Password");

            }
            return BadRequest(ModelState);
        }

        #endregion



        #region get user info

        [Authorize]

        [HttpGet("GetUserInfo")] //http://localhost:5267/api/Account/GetUserInfo

        public async Task<IActionResult> GetUserInfo()

        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var Role = User.FindFirst(ClaimTypes.Role)?.Value;


            if (userId == null)

                return Unauthorized();

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)

                return NotFound();

            var compLocation = communityRepo.Get(userId);

            return Ok(new

            {

                user.Id,

                user.UserName,

                user.Email,

                user.firstName,

                user.lastName,

                user.image,

                user.PhoneNumber,
                compLocation,
                user.communityId,

                Role


            });

        }

        #endregion

        #region requestpasswordReset
        [HttpGet("requestPasswordreset/{email}")]
        public async Task<IActionResult> RequestPasswordReset(string email)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
                      
            if (user == null)
            {
                return BadRequest(new { sucess = false, message = "No User found with this email" });
            }
            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user!);
            string validToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));
            await SendPasswordResetEmail(user.Email, validToken);

            return Ok(new { sucess = true, message = "Check your email for password reset link." }); 

        }

        private async Task SendPasswordResetEmail(string? email, string validToken)
        {
            var audienceIP = Config["JWT:AudienceIP"]?.TrimEnd('/'); ;
            string resetLink = $"{audienceIP}/reset-password?email={email}&token={validToken}";

            StringBuilder sb = new();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset=\"UTF-8\">");
            sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            sb.AppendLine("<title>Password Reset</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("    body { font-family: Arial, sans-serif; background-color: #f4f4f9; padding: 20px; }");
            sb.AppendLine("    .email-container { max-width: 600px; margin: 0 auto; background-color: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }");
            sb.AppendLine("    h1 { color: #333; }");
            sb.AppendLine("    a {text-decoration: none;color: gainsboro;}");
            sb.AppendLine("    p { color: #555; }");
            sb.AppendLine("    .button { background-color: #3a74b3; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block; }");
            sb.AppendLine("    .button:hover { background-color: #2a5a93; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <div class=\"email-container\">");
            sb.AppendLine($"        <h1>Hello, {email}</h1>");
            sb.AppendLine("        <p>You requested to set a new password for your account. Click the button below to set your password:</p>");
            sb.AppendLine($"        <p><a href=\"{resetLink}\" class=\"button\">Set Your Password</a></p>");
            sb.AppendLine("        <p>If you didn’t request this, please ignore this email.</p>");
            sb.AppendLine("        <p>Thank you,</p>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            string message = sb.ToString();
            await _emailService.SendEmailAsync(
               toEmail: email,
               subject: "password Reset",
               htmlBody:message
           );
        }

        #endregion

        #region password reset
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
                return BadRequest(new { Sucess = false, message = "Invalid request." });

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));

            var result = await userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

            if (result.Succeeded)
                return Ok(new { Sucess = true, message = "Password has been reset successfully." });

            return BadRequest(result.Errors); 
        }

        #endregion

        #region editImage
        [Authorize]
        [HttpPost("editImage")]
        [RequestSizeLimit(104_857_600)] // 100 MB
        [RequestFormLimits(MultipartBodyLengthLimit = 104_857_600)]
        public async Task<IActionResult> EditImage([FromForm] EditImageDTO editImage)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "User not found." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            if (editImage.image != null && editImage.image.Length > 0)
            {
                //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(editImage.image.FileName);
                //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await editImage.image.CopyToAsync(stream);
                //}

                //if (!string.IsNullOrEmpty(user.image))
                //{
                //    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images",user.image);
                //    if (System.IO.File.Exists(oldImagePath))
                //        System.IO.File.Delete(oldImagePath);
                //}

                //delete old one if exists  
                if (!string.IsNullOrEmpty(user.image))
                {
                    await cloudinaryRepository.DeleteImageAsync(user.image);
                }
                //Upload to Cloudinary
                var fileName = await cloudinaryRepository.UploadImageAsync(editImage.image);
                    
                user.image = fileName;
                await userManager.UpdateAsync(user);
                return Ok(new {success=true, message = "Image updated successfully",data=fileName });
            }

            return BadRequest(new { success = false, message = "No Image Upload." });
        }
        #endregion
    }
}
