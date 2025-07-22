using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.DTO.Auth;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(RoleDTO roleVM)
        {
            if (ModelState.IsValid)
            {
                //create role
                IdentityRole role = new IdentityRole()
                {
                    Name = roleVM.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Ok("Role is added");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);

                }
            }
            return BadRequest(ModelState);
        }

    }
}
