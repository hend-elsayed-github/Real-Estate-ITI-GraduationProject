using Real_Estatae_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class CommunityPost
    {
        public int id { get; set; }

        public string content { get; set; }

        public bool isDeleted { get; set; }=false;

        public DateTime publishDate { get; set; } = DateTime.Now;
        public string? image { get; set; }

        //community-communityPost 1-m(communityPost)
        [ForeignKey("community")]
        public int communityId { get; set; }
        public virtual Community community { get; set; }  
        
        
        [ForeignKey("ApplicationUser")]
        public string userId { get; set; }
        public virtual ApplicationUser ApplicationUser  { get; set; }



        //comment-communityPost 1-m(comment)
        public virtual List<Comment> Comments { get; set; }
        //react-communityPost 1-m(react)

        public virtual List<React> React { get; set; } = new();

    }
}
