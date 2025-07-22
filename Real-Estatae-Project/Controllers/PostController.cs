using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.DTO;
using Real_Estatae_Project.DTO.Post;
using Real_Estatae_Project.Images;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;
using System.Data;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _IUserRepository;

        public PostController(IPostRepository postRepository, IUserRepository UserRepository)
        {
            _postRepository = postRepository;
            _IUserRepository = UserRepository;
        }


        #region get all posts

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllPostsDTO>>> GetAllPosts()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

             string userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            int? communityId;

            communityId = await _IUserRepository.GetCommunityId(userId, userRole);
            if (communityId == null)
                return BadRequest(new { message = "User not assigned to a community." });

            var clients = await _postRepository.GetAllPosts(userId, communityId);
            var PostsDto = clients.Select(p => new AllPostsDTO
            {
                PostId = p.id,
                content = p.content,
                publishDate = p.publishDate,
                PostImage = p.image,
                reactCount=p.React.Count(),
                UserName = p.ApplicationUser.firstName + " " + p.ApplicationUser.lastName,
                userRole = userRole,
                UserImage = p.ApplicationUser.image,
                commentCount=p.Comments.Where(c =>  !c.isDeleted).Count()
 


            }).ToList();


            return Ok(PostsDto);
        }

        #endregion


        #region Add Post

        [HttpPost]
        public async Task<IActionResult> AddPost([FromForm] PostInfoDTO postinfo)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            string userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            int? communityId;

             communityId =  await _IUserRepository.GetCommunityId(userId, userRole);
            if (communityId == null)
                return BadRequest(new { message = "User not assigned to a community." });

            string? imageFromReq = await GetImageName.GetImageNameFn(postinfo.image);
            var post = new CommunityPost
            {
                content = postinfo.content,
               communityId=(int) communityId,
                    image = imageFromReq,
                    userId = userId

            };

            var createdPostid = await _postRepository.Add(post);

            return Ok(new
            {
                message = "post added successfully.",
                PostId = createdPostid
            });


        }
        #endregion

        #region Delete post

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var deleted = await _postRepository.Delete(id, userId);

            if (deleted == false)
                return NotFound(new { message = "post not found or already deleted." });

            return Ok(new { message = "post deleted successfully" });
        }

        #endregion

        #region update post
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] PostInfoDTO postdto)
        {


            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingPost = await _postRepository.GetById(id);
            if (existingPost == null || existingPost.userId != userId)
                return NotFound(new { message = "Post not found or access denied." });

     

            string? imageFromReq = await GetImageName.GetImageNameFn(postdto.image);
            string? imageUrl = imageFromReq != null ? imageFromReq : existingPost.image;
            var updatedpost = new CommunityPost
            {
                content = postdto.content,
                image = imageUrl
            };

            var result = await _postRepository.Update(id, updatedpost, userId);

            if (result == 0)
                return NotFound(new { message = "post not found or you don't have access." });

            return Ok(new { message = "post updated successfully.", id = result });


        }

        #endregion


        #region
        [HttpGet("{id}")]
        public async Task<ActionResult<PostByIdDTO>> getbyId(int id)
        {
            string OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (OwnerId == null)
                return Unauthorized();

            CommunityPost Post = await _postRepository.GetById(id);
            if (Post.userId != OwnerId || Post == null)
            {
                return NotFound();
            }
            PostByIdDTO updatePost = new PostByIdDTO
            {
                Content = Post.content,
                Image = Post.image
            };
            return Ok(updatePost);

        }
        #endregion
    }
}
