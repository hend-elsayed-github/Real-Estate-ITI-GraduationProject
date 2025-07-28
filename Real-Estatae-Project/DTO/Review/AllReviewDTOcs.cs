using Real_Estate_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estatae_Project.DTO.Review
{
    public class AllReviewDTOcs
    {
        public int id { get; set; }
        public string rate { get; set; } 

        public string content { get; set; }

        public DateTime publishDate { get; set; } = DateTime.Now;

        public string fullName { get; set; }
        public string userName { get; set; }
        public string userImage { get; set; }
    }
}
