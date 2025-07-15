using Real_Estate_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estatae_Project.Models
{
    public class React
    {
    

             public int Id { get; set; }

            [ForeignKey("User")]
            public string UserId { get; set; }
            public virtual ApplicationUser User { get; set; }
            [ForeignKey("Post")]
            public int PostId { get; set; }
            public virtual CommunityPost Post { get; set; }

        
    }
}
