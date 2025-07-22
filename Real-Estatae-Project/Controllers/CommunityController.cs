using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.DTO;
using Real_Estatae_Project.DTO.Community;
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
        private readonly IUserRepository userRepository;
        public CommunityController(ICommunityRepository communityRepository,IUserRepository _userRepository)
        {
            _communityRepository = communityRepository;
            userRepository = _userRepository;
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


     //   #endregion
        //#endregion

        #region update
        [HttpPut]
        public IActionResult UpdateCommunity( CommunityInfoDTO communityInfoDTO)
        {
            var ownerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(ownerId))
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isUpdated = _communityRepository.Update(ownerId, communityInfoDTO);
            if (isUpdated)
                return Ok(new { message = "Community updated successfully" });
            return NotFound(new { message = "Community not found" });
        }
        #endregion


        #region get name
        [HttpGet]
        public IActionResult GetCommunityName()
        {
            var ownerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(ownerId))
                return Unauthorized();

            var communityId = _communityRepository.GetCommunityId(ownerId);
            if (communityId == 0)
                return NotFound(new { message = "Community not found" });
            string communityName = _communityRepository.GetName(communityId);
            if (communityName == null)
                return NotFound(new { message = "Community not found" });
            return Ok(new { name = communityName });
        }
        #endregion


        /////////////////////////
        #region usercommunity
        [HttpGet("usercommunity")]

        public async Task<IActionResult> GetUserCommunity()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserCommunityDTO user = await userRepository.GetUserCommunity(userId);
            if (user == null)
            {
                return NotFound();

            }
            return Ok(user);

        }

        #endregion
        #region Top5
        [HttpGet("topActive")]
        public async Task<ActionResult<List<TopUserDTO>>> GetTopActiveUsersInCommunity()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<UserCommunityDTO> users = await userRepository.GetTopActiveUsersByCommunityAsync(userId);

            List<TopUserDTO> topUsers = new List<TopUserDTO>();
            foreach (var user in users)
            {
                TopUserDTO top = new TopUserDTO
                {
                    Image = user.Image,
                    Name = user.Name
                };
                topUsers.Add(top);
            }


            return topUsers;

        }
        #endregion

    }
}
