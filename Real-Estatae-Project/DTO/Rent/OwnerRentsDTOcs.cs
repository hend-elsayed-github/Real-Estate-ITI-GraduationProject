namespace Real_Estatae_Project.DTO.Rent
{
    public class OwnerRentsDTOcs
    {
        public int RentId { get; set; }
        public double RentValue { get; set; }
        public string RentStatus { get; set; }
        public DateOnly DueDate { get; set; }

        public string RenterName { get; set; }
        public DateOnly? PaymentDate { get; set; }
    }
}
