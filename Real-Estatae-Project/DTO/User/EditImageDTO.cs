using Real_Estatae_Project.Custom_Validation;

namespace Real_Estatae_Project.DTO.User
{
    public class EditImageDTO
    {
        [ValidImage(ErrorMessage = "Only image files (.jpg, .png, etc.) are allowed.")]
        public IFormFile? image { get; set; }
    }
}
