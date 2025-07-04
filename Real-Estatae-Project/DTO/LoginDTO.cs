using System.ComponentModel.DataAnnotations;

namespace Real_Estatae_Project.DTO
{
    public class LoginDTO
    {
        public string userName { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }

    }
}
