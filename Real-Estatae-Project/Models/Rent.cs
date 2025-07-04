using Real_Estatae_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class Rent : BillParent
    {

        public DateOnly paymentDate { get; set; }

        public DateOnly dueDate { get; set; }

        //need enum
        public string status { get; set; }

        // unit -rent 1-1
        [ForeignKey("unit")]
        public int unitId { get; set; }
        public virtual Unit unit { get; set; }
    }
}
