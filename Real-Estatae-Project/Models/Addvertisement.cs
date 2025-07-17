using Real_Estatae_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class Addvertisement
    {
        public int id {  get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public DateTime publishDate { get; set; } = DateTime.Now;

        public bool isDeleted { get; set; }

        // owner 1-m 
        public string userId { get; set; }
        public virtual ApplicationUser user { get; set; }

        // addvirtisement - unit 1-1
        [ForeignKey("unit")]
        public int unitId { get; set; }
        public virtual Unit unit { get; set; }

        // advertisement - appointment 1-m

        public virtual List<Appointment> Appointments { get; set; } 


    }
}
