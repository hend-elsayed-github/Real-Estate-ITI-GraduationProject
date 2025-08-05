using Real_Estatae_Project.DTO.Admin;

namespace Real_Estatae_Project.Repositories
{
    public interface IAdminRepository
    {
         Task<List<UserDTO>> GetAll();
        Task<List<OwnerDTO>> GetAllOwners();
        Task<List<RenterDTO>> GetAllRenters();
        Task<object> GeneralNumbers();
        Task<List<ProfitDTO>> GetProfitMonth();
        Task Transfer(string oldOwner, string newOwner);
    }
}
