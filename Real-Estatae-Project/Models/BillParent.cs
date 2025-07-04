using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Models
{
    public class BillParent
    {
        public int id { get; set; }

        public double value { get; set; }

        // billParent- payment 1-1
        public virtual Payment payment { get; set; }
    }
}
