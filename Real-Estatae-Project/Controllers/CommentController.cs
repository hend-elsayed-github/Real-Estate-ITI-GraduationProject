using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.DTO.Comment;
using Real_Estatae_Project.Hubs;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {

        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;


        public CommentController(ICommentRepository CommentRepository, IPostRepository postRepository, IUserRepository UserRepository, INotificationRepository NotificationRepository, IHubContext<NotificationHub> hubContext)
        {
            _commentRepository = CommentRepository;
            _postRepository = postRepository;

            _userRepository = UserRepository;
            _notificationRepository = NotificationRepository;
            _hubContext = hubContext;
        }


        #region Add comment

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentInfoPostIdDTO commentinfo)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
                
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var postExists = await _postRepository.PostExists(commentinfo.PostId);
            if (!postExists)
                return NotFound(new { message = "Post not found or has been deleted" });


            var comment = new Comment
            {
                content = commentinfo.content,
                communityPostId = commentinfo.PostId,
                userId = userId


            };

            var CreatedCommentId = await _commentRepository.Add(comment);

            //notification

            var post = await _postRepository.GetById(commentinfo.PostId);
            string postOwnerId = post.userId;

            var commenter = await _userRepository.FindByIdAsync(userId);
            string commenterName = commenter.firstName + " " + commenter.lastName;


              if (postOwnerId != userId)
              {
              string message = $"{commenterName} commented on your post";


         var notification = new Notification
       {
           userId = postOwnerId,
           message = message,
           sender = commenterName
       };
       await _notificationRepository.AddAsync(notification);

       //  SignalR
       await _hubContext.Clients.User(postOwnerId)
           .SendAsync("ReceiveNotification", new
           {
               message = message,
               sender = commenterName,
               createdAt = DateTime.UtcNow
           });
   }
            return Ok(new
            {
                message = "Comment added successfully.",
                commentId = CreatedCommentId
            });

            // return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);

        }
        #endregion


        #region update comment
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, CommentInfoDTO commentdto)
        {


            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedcomment = new Comment
            {
                content = commentdto.content,

            };

            var result = await _commentRepository.Update(id, updatedcomment, userId);

            if (result == 0)
                return NotFound(new { message = "comment not found or you don't have access." });

            return Ok(new { message = "comment updated successfully.", id = result });

            // return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);

        }

        #endregion

        #region Delete Commentt

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var deleted = await _commentRepository.Delete(id, userId);

            if (deleted == false)

                return NotFound(new { message = "comment not found or already deleted." });

            return Ok(new { message = "comment deleted successfully" });

        }

        #endregion


        #region get all comments

        [HttpGet("{id}")]

        public async Task<ActionResult<IEnumerable<AllCommentDTO>>> GetAllComments(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var postExists = await _postRepository.PostExists(id);
            if (!postExists)
                return NotFound(new { message = "Post not found or has been deleted" });


            var comments = await _commentRepository.GetAll(id);
            var commentDto = comments.Select(p => new AllCommentDTO
            {
                CommentId = p.id,
                content = p.content,
                publishDate = p.publishDate,
                UserName = p.user.firstName + " " + p.user.lastName,
                UserImage = p.user.image

            }).ToList();


            return Ok(commentDto);
        }
        #endregion

    }

}

