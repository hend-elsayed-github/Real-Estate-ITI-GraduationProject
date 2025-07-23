namespace Real_Estatae_Project.DTO.Rent
{
    public class RentHistoryfoDTO
    {
     
       
        public double RentValue { get; set; }
        public string RentStatus { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly? PaymentDate { get; set; }

    


    }


}
