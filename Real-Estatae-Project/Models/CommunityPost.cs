using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class CommunityPost
    {
        public int id { get; set; }

        public string content { get; set; }

        public bool isDeleted { get; set; }

        public DateTime publishDate { get; set; } = DateTime.Now;

        //community-communityPost 1-m(communityPost)
        [ForeignKey("community")]
        public int communityId { get; set; }
        public virtual Community community { get; set; }


        //comment-communityPost 1-m(comment)
        public virtual List<Comment> Comments { get; set; } 
    }
}
