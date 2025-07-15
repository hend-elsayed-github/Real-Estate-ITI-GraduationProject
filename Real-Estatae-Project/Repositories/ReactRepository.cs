using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.Models;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class ReactRepository:IReactRepository
    {
        private readonly ProjectContext _context;
        public ReactRepository(ProjectContext _Context)
        {
            _context = _Context;
        }
        #region Toggle React
        public async Task<string> ToggleReactAsync(React react)
        {

            var existingReact = await _context.Reacts.FirstOrDefaultAsync(r =>
                r.UserId == react.UserId &&
                r.PostId == react.PostId
                );
                  
                if (existingReact != null)
                {
                    _context.Reacts.Remove(existingReact);
                    await _context.SaveChangesAsync();
                    return "removed";
                }
                _context.Reacts.Add(react);
                await _context.SaveChangesAsync();
                return "added";
            }

        
        #endregion
    }
}
