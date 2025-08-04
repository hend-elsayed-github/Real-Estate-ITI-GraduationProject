namespace Real_Estatae_Project.DTO.Admin
{
    public class UserDTO
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string? communityName { get; set; }

        //for owners
        public int? unitCount { get; set; }
        public int? adCount { get; set; }

    }
}
