namespace Real_Estatae_Project.DTO
{
    public class UserCommunityDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public int CommentCount { get; set; }

        public int ReactCount {  get; set; }
        public int PostCount {  get; set; }
    }
}
