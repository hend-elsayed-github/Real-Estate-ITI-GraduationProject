using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.DTO;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

   // [Authorize(Roles = "owner")]

    public class CommunityController : ControllerBase

    {
        private readonly ICommunityRepository _communityRepository;
        public CommunityController(ICommunityRepository communityRepository)
        {
            _communityRepository = communityRepository;
        }


        //#region create commumity
        //[HttpPost]
        //public async Task<IActionResult> CreateCommunity([FromBody] CommunityInfoDTO CommunityInfoDTO)
        //{
        //    var ownerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(ownerId))
        //        return Unauthorized();
           
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var community = new Community
        //    {
        //        name = CommunityInfoDTO.Name,
        //        ownerId = ownerId
        //    };

        //    var id = await _communityRepository.Create(community);

        //    return Ok(new { message = "Community created", id });

        //}


        //#endregion

    }
}
