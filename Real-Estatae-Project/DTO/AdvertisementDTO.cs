using Real_Estatae_Project.Models;
using Real_Estate_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estatae_Project.DTO
{
    public class AdvertisementDTO
    {

        public string title { get; set; }

        public string description { get; set; }

        public int unitId { get; set; }
       
    }
}
