using System.ComponentModel.DataAnnotations.Schema;
using Real_Estatae_Project.Models;

namespace Real_Estate_Project.Models
{
    public class Maintenance
    {
        public int id { get; set; }
        public double value { get; set; }
        public string specialist  { get; set; }
        public string name { get; set; }

        public double totalValue { get; set; }

        public string description { get; set; }

        public bool isDeleted { get; set; }

        // maintenance-unit 1-m(maintenance)

        [ForeignKey("unit")]
        public int unitId { get; set; }
        public virtual Unit unit { get; set; }

        // billParent-maintenance 1-1

        [ForeignKey("billParent")]
        public int billId { get; set; }
        public virtual BillParent billParent { get; set; }

    }
}
