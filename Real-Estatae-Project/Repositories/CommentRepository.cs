using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.Models;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class CommentRepository:ICommentRepository
    {

        private readonly ProjectContext _context;
        public CommentRepository(ProjectContext _Context)
        {
            _context = _Context;
        }

        #region add comment
        public async Task<int> Add(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment.id;

        }
        #endregion
       
       

        #region update post
        public async Task<int> Update(int CommentId, Comment updatedComment, string userId)
        {

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.id == CommentId && !c.isDeleted && c.userId == userId);

            if (comment == null)
                return 0;

            comment.content = updatedComment.content;

           
            await _context.SaveChangesAsync();

            return comment.id;


        }


        #endregion

        #region Delete comment
        public async Task<bool> Delete(int id, string userId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.id == id && !c.isDeleted && c.userId == userId);

            if (comment == null)
                return false;

            comment.isDeleted = true;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion


        #region get all comments


        public async Task<IEnumerable<Comment>> GetAll(int postId)
        {
            var postExists = await _context.CommunityPosts
                .AnyAsync(p => p.id == postId && !p.isDeleted);

            if (!postExists)
                return new List<Comment>();

            var comments = await _context.Comments
                .Where(c => c.communityPostId == postId && !c.isDeleted)
                .Include(c => c.user)
                .OrderByDescending(c => c.publishDate)
                .ToListAsync();

            return comments;
        }



        #endregion

    }
}
