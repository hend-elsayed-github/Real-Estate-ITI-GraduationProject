using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.DTO;
using Real_Estatae_Project.Models;
using Real_Estatae_Project.Repositories;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReactController : ControllerBase
    {

        private readonly IReactRepository _reactRepository;
        private readonly IPostRepository _postRepository;
        public ReactController(IReactRepository CommentRepository, IPostRepository reactRepository)
        {
            _reactRepository = CommentRepository;
            _postRepository = reactRepository;
        }

        #region Toggle React
        [HttpPost]
        public async Task<IActionResult> ToggleReaction( ReactDTO reactDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var postExists = await _postRepository.PostExists(reactDTO.PostId);
            if (!postExists)
                return NotFound(new { message = "Post not found or has been deleted" });

            var react = new React
            {
                UserId = userId,
                PostId = reactDTO.PostId
               
            };

            var result = await _reactRepository.ToggleReactAsync(react);

            return Ok(new { message = $"react {(result == "added" ? "added" : "removed")}" });
        }

        #endregion

    }
}
