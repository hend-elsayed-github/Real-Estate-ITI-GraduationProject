using System.ComponentModel.DataAnnotations;

namespace Real_Estatae_Project.DTO.Auth
{
    public class AdminDTO
    {
        public string userName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }

        [Compare("password")]
        [DataType(DataType.Password)]

        [Display(Name = "Confirm Password")]
        public string confirmPassword { get; set; }
        
    }
}
