namespace Real_Estatae_Project.DTO.Review
{
    public class AllReviewDTO
    {
        public int id { get; set; }
        public string rate { get; set; }

        public string content { get; set; }

        public DateTime publishDate { get; set; } = DateTime.Now;

        public string fullName { get; set; }
        public string userName { get; set; }
        public string userImage { get; set; }
        public string communityName { get; set; }
    }
}
