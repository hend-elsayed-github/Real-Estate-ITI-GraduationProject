namespace Real_Estatae_Project.DTO.Post
{
    public class AllPostsDTO
    {

        public int PostId { get; set; }
        public string content { get; set; }

       
        public string? PostImage { get; set; }

        public int reactCount { get; set; }
        public DateTime publishDate { get; set; }

        public string UserName { get; set; }
        public string? UserImage { get; set; }

        public string? userRole { get; set; }

        public int commentCount {get; set;}


    




    }
}
