namespace Real_Estatae_Project.DTO
{
    public class AddUnitsDTO
    {

        public string status { get; set; } 
        public double price { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public string? area { get; set; }
        public string? flatNumber { get; set; }
        public string? buildingNumber { get; set; }

        public IFormFile? image1 { get; set; }
        public IFormFile? image2 { get; set; }
        public IFormFile? image3 { get; set; }

        public string electricityNum { get; set; }
        public string waterNum { get; set; }
        public string gasNum { get; set; }

        
    }
}
