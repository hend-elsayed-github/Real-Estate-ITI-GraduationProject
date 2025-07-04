using System.ComponentModel.DataAnnotations;

namespace Real_Estatae_Project.ViewModels
{
    public class RoleDTO
    {
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
