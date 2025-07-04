using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class Comment
    {
        public int id { get; set; }

        public string content { get; set; }

        public bool isDeleted { get; set; }

        public DateTime publishDate { get; set; } = DateTime.Now;

        //comment-communityPost 1-m(comment)
        [ForeignKey("communityPost")]
        public int communityPostId { get; set; }
        public virtual CommunityPost communityPost { get; set; }

        // user 1-m
        public string userId { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}
