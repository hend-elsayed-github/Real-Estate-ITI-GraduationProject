using Real_Estatae_Project.Models;
using Real_Estate_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estatae_Project.DTO.Advertisement
{
    public class AdvertisementDTO
    {
        public int AdID { get; set; }
        public string title { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public string type { get; set; }

        public string city { get; set; }
        public string street { get; set; }
        public string? area { get; set; }
        public string? flatNumber { get; set; }
        public string? buildingNumber { get; set; }
        public string? image1 { get; set; }
        public string? image2 { get; set; }
        public string? image3 { get; set; }
        public DateTime publishDate { get; set; }

        public int unitId { get; set; } 

        public string communityName { get; set; } 

    }
}
