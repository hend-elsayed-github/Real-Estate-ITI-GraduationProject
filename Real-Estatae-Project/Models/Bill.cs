using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Real_Estatae_Project.Models;

namespace Real_Estate_Project.Models
{
    public class Bill
    {
        public int  id { get; set; }        
        public double value { get; set; }
        public string type { get; set; }

        public string status { get; set; }

        public DateTime? expirationDate { get; set; }

        // bill-unit 1-m(bill)
        [ForeignKey("unit")]
        public int unitId { get; set; }
        public virtual Unit unit { get; set; }

        // billParent-bill 1-1

        [ForeignKey("billParent")]
        public int billId { get; set; }
        public virtual BillParent billParent { get; set; }


    }
}
