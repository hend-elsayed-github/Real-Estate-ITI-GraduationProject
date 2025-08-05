namespace Real_Estatae_Project.DTO.Admin
{
    public class AllReserDTO
    {
        public int id { get; set; }

        public string Status { get; set; }
        public DateTime reservationDate { get; set; }
        public string Location { get; set; }
        public string name { get; set; }

        public string phoneNumber { get; set; }

        public string email { get; set; }
        public int appointmentId { get; set; }
        public string owner { get; set; }
    }
}
