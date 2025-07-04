using Real_Estate_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estatae_Project.Models
{
    public class BillParent
    {
        public int id { get; set; }

        public double value { get; set; }

        // billParent- payment 1-1
        public virtual Payment payment { get; set; }


        // billParent-bill 1-1

        public virtual Bill bill { get; set; }

        // billParent-rent 1-1

        public virtual Rent rent { get; set; }

        // billParent-maintenance 1-1

        public virtual Maintenance maintenance { get; set; }
    }
}
