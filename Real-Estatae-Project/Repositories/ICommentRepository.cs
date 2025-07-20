using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {


        public Task<int> Add(Comment Post);
        public Task<bool> Delete(int id, string userId);
        Task<int> Update(int id, Comment updatedPost, string userId);
        Task<IEnumerable<Comment>> GetAll(int PostId);

    }
}
