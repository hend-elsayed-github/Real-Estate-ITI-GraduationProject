using Real_Estate_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estatae_Project.DTO.Unit
{
    public class AllUnit
    {

        public int id { get; set; }
        public string status { get; set; }
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
        public string? renterSSN { get; set; }
        public string electricityNum { get; set; }

        public string waterNum { get; set; }

        public string gasNum { get; set; }
        public bool isAds { get; set; } = false;

    }
}
