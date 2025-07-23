using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class Review
    {
        public int id { get; set; }
        public string rate { get; set; } // e.g. "5 stars", "4 stars", etc.

        public string content   { get; set; } 

        public DateTime publishDate { get; set; } = DateTime.Now; 

        public bool isDeleted { get; set; } 

        // review-unit 1-m(review) cancelled

        //[ForeignKey("unit")]
        //public int unitId { get; set; }
        //public virtual Unit unit { get; set; }

        //review - community 1-m reviews
        [ForeignKey("community")]
        public int communityId { get; set; }
        public virtual Community community { get; set; }

        // renter 1-m reviews
        [ForeignKey("renter")]
        public string userId { get; set; }
        public virtual ApplicationUser renter { get; set; }
    }
}
