namespace Real_Estatae_Project.DTO.Appointment
{
    public class AllAppointmentDTO
    {
        public int id { get; set; }
        public DateTime dateTime { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public string? area { get; set; }
        public string? flatNumber { get; set; }
        public string? buildingNumber { get; set; }
    }
}
