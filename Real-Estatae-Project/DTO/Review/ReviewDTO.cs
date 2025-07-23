namespace Real_Estatae_Project.DTO.Review
{
    public class ReviewDTO
    {
        public string rate { get; set; } // e.g. "5 stars", "4 stars", etc.

        public string content { get; set; }

        public int communityId { get; set; }

        public string renterId { get; set; } 
    }
}
