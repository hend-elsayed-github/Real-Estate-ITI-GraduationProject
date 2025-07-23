using System.ComponentModel.DataAnnotations;

namespace Real_Estatae_Project.DTO.Auth
{
    public class RoleDTO
    {
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
