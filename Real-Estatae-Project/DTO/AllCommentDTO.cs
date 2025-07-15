namespace Real_Estatae_Project.DTO
{
    public class AllCommentDTO
    {

        public int CommentId { get; set; }
        public string content { get; set; }


        public DateTime publishDate { get; set; }

        public string UserName { get; set; }
        public string? UserImage { get; set; }

    }
}
