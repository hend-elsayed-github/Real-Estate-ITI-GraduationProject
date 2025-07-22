
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class Community
    {
        public int id { get; set; }

        public string? name { get; set; }

        //community-unit 1-m(unit)
        public  virtual List<Unit>? Units {  get; set; }

        //community-communityPost 1-m(communityPost)
        public virtual List<CommunityPost>? CommunityPosts { get; set; }

        // renter M-1

        public virtual List<ApplicationUser>? renters { get; set; }

        //community 1- 1 owner >>>>>1 owner has one community
        [ForeignKey("Owner")]
        public string ownerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        // review-community 1-m(review)
        public virtual List<Review>? Reviews { get; set; }
    }
}
