using Real_Estatae_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface IReactRepository:IRepository<React>
    {
        Task<string> ToggleReactAsync(React react);

    }
}
