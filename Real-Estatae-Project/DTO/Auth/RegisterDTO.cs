using Real_Estatae_Project.Custom_Validation;
using System.ComponentModel.DataAnnotations;

namespace Real_Estatae_Project.DTO.Auth
{
    public class RegisterDTO
    {
        public string firstName {  get; set; }
        public string lastName { get; set; }

        public string userName { get; set; }

        public string? phone {  get; set; }

        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }

        [Compare("password")]
        [DataType(DataType.Password)]

        [Display(Name = "Confirm Password")]
        public string confirmPassword { get; set; }

        [ValidImage(ErrorMessage = "Only image files (.jpg, .png, etc.) are allowed.")]
        public IFormFile? imageFile {  get; set; }

        public string role { get; set; }
    }
}
