using Real_Estatae_Project.Custom_Validation;

namespace Real_Estatae_Project.DTO.Post
{
    public class PostInfoDTO
    {

        public string content { get; set; }

        [ValidImage(ErrorMessage = "Only image files (.jpg, .png, etc.) are allowed.")]
        public IFormFile? image { get; set; }

    }
}
