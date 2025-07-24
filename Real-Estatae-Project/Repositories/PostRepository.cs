using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.Hubs;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class PostRepository :IPostRepository
    {

        private readonly ProjectContext _context;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public PostRepository(ProjectContext context)
        {
            _context = context;
        }

        #region add post
        public async Task<int> Add(CommunityPost Post)
        {
            await _context.CommunityPosts.AddAsync(Post);
            await _context.SaveChangesAsync();
            return Post.id;

        }
        #endregion
        #region Delete post
        public async Task<bool> Delete(int id,string userId)
        {
            var post = await _context.CommunityPosts.FirstOrDefaultAsync(p => p.id == id && !p.isDeleted&& p.userId == userId);

            if (post == null)
                return false;

            post.isDeleted = true;
            _context.CommunityPosts.Update(post);
            await _context.SaveChangesAsync();
            return true;
        }



        #endregion

        #region update post
        public async Task<int> Update(int id,CommunityPost updatedPost, string userId)
        {

           
           var post = await _context.CommunityPosts.FirstOrDefaultAsync(p => p.id == id && !p.isDeleted && p.userId == userId);

                if (post == null)
                    return 0 ;

                post.content = updatedPost.content;
                post.image=updatedPost.image;
                
                await _context.SaveChangesAsync();

                return post.id;

            
        }


        #endregion

        #region get all posts
        public async Task<IEnumerable<CommunityPost>> GetAllPosts(string userId ,int? communityId)
        {

            var user = await _context.Users
                    .Include(u => u.RenterCommunity)
                    .FirstOrDefaultAsync(u => u.Id == userId);

              if (user == null || communityId == null)
                return new List<CommunityPost>();


            var posts = await _context.CommunityPosts
                .Where(p => p.communityId == communityId && !p.isDeleted)
                .Include(p => p.ApplicationUser)
                .Include(p=>p.React)
                .OrderByDescending(c => c.publishDate)
                .ToListAsync();

            return posts;
        }



        #endregion

        #region Is Post Exists
        public async Task<bool> PostExists(int postId)
        {
            return await _context.CommunityPosts
                .AnyAsync(p => p.id == postId && !p.isDeleted);
        }
        #endregion


        #region GetById
        public async Task<CommunityPost> GetById( int postId )
        {
            
            return await _context.CommunityPosts
                 .FirstOrDefaultAsync(p => p.id == postId  && !p.isDeleted);
        }

        #endregion




    }
}
