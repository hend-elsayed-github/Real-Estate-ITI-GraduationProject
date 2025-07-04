using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Real_Estatae_Project.DTO;
using Real_Estate_Project.Models;
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

        public IConfiguration Config { get; }

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration Config)
        {
            this.userManager = userManager;
            this.Config = Config;
        }



        #region Registeration
        [HttpPost("Register")] //api/Account/Register
        public async Task<IActionResult> Register(RegisterDTO userFromRequest)
        {
            if (ModelState.IsValid)
            {
                //Create user ==> save in DB 
                ApplicationUser user = new ApplicationUser();
                user.UserName = userFromRequest.userName;
                user.Email = userFromRequest.email;
                user.firstName = userFromRequest.firstName;
                user.lastName = userFromRequest.lastName;
                user.PhoneNumber = userFromRequest.phone;
                IdentityResult reuslt = await userManager.CreateAsync(user, userFromRequest.password);

                if (userFromRequest.role == "Owner")
                {
                    await userManager.AddToRoleAsync(user, "Owner");

                }
                else if (userFromRequest.role == "Renter")
                {
                    await userManager.AddToRoleAsync(user, "Renter");
                }
                else
                {
                    await userManager.AddToRoleAsync(user, "Visitor");

                }

                // image part
                IFormFile? imageFile = userFromRequest.imageFile;

                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    user.image = fileName;
                }

                if (reuslt.Succeeded)
                {
                    return Ok("Created");
                }
                foreach (var error in reuslt.Errors)
                {
                    ModelState.AddModelError("Errors", error.Description);
                }

            }
            return BadRequest(ModelState);
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
                            expiration = DateTime.Now.AddHours(1) // OR myTokn.expires
                        });
                    }

                }
                ModelState.AddModelError("Password", "Invalid UserName OR Password");

            }
            return BadRequest(ModelState);
        }

        #endregion



    }
}
