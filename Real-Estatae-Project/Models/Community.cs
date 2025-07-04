
namespace Real_Estate_Project.Models
{
    public class Community
    {
        public int id { get; set; }

        public string name { get; set; }

        //community-unit 1-m(unit)
        public  virtual List<Unit> Units {  get; set; }

        //community-communityPost 1-m(communityPost)
        public virtual List<CommunityPost> CommunityPosts { get; set; }

        // user 1-m
        public string userId { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}
