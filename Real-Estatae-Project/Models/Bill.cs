using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Real_Estatae_Project.Models;

namespace Real_Estate_Project.Models
{
    public class Bill:BillParent
    {
        public string type { get; set; }

        public string status { get; set; }

        public DateTime? expirationDate { get; set; }

        // bill-unit 1-m(bill)
        [ForeignKey("unit")]
        public int unitId { get; set; }
        public virtual Unit unit { get; set; }

        
    }
}
