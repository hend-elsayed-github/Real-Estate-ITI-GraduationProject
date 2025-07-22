using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Real_Estatae_Project.DTO.Auth;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;
using Stripe;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        public ICommunityRepository communityRepo;
        public IConfiguration Config { get; }

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration Config, ICommunityRepository _communityRepo)
        {
            this.userManager = userManager;
            this.Config = Config;
            this.communityRepo = _communityRepo;
        }



        #region Registeration
        [HttpPost("Register")] // api/Account/Register
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
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userFromRequest.imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await userFromRequest.imageFile.CopyToAsync(stream);
                }

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
                    Type = "standard"
                });

              
                user.StripeAccountId = stripeAccount.Id;

                await userManager.UpdateAsync(user);
                Community newComm = new Community
                {
                    name = "new community",
                    ownerId = user.Id 
                };

                communityRepo.Create(newComm);
                communityRepo.Save(); 
            }

            return Ok("User registered successfully");
        }

        #endregion


        #region login

        [HttpPost("Login")]  //api/Account/Login
        public async Task<IActionResult> Login(LoginDTO userFromRequest)
        {
            if (ModelState.IsValid)
            {
                //Check user in DB? 

                ApplicationUser userFromDB = await userManager.FindByNameAsync(userFromRequest.userName);

                if (userFromDB == null)
                {
                    userFromDB = await userManager.FindByEmailAsync(userFromRequest.email);
                }

                if (userFromDB != null)
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
                            expiration = DateTime.Now.AddHours(24) // OR myTokn.expires
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

            if (userId == null)
                return Unauthorized();

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.firstName,
                user.lastName,
                user.image,
                user.PhoneNumber

            });
        }
        #endregion

    }
}
