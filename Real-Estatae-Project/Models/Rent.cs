using Real_Estatae_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class Rent 
    {//   status
        public int id { get; set; }
     


        public DateOnly dueDate { get; set; }

        //need enum
        //public string status { get; set; }
        public bool IsPaid { get; set; }=false;
        public double Rentvalue { get; set; }

        // unit -rent 1-1
        [ForeignKey("unit")]
        public int unitId { get; set; }
        public virtual Unit unit { get; set; }

       
        public virtual Payment? Payment { get; set; }

    }
}
