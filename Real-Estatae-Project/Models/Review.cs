using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class Review
    {
        public int id { get; set; }
        public string rate { get; set; }

        public string content   { get; set; }

        public DateTime publishDate { get; set; } = DateTime.Now;

        public bool isDeleted { get; set; }

        // review-unit 1-m(review)

        [ForeignKey("unit")]
        public int unitId { get; set; }
        public virtual Unit unit { get; set; }

        // user 1-m
        public string userId { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}
