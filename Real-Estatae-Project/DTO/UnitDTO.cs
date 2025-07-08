namespace Real_Estatae_Project.DTO
{
    public class UnitDTO
    {
        public string status { get; set; }  //empty or rented
        public double price { get; set; }
        public string description { get; set; }
        public string type { get; set; } //for rent or already sold
        public string? image1 { get; set; }
        public string? image2 { get; set; }
        public string? image3 { get; set; }

    }
}
