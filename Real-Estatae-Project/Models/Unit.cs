using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class Unit
    {
        public int id { get; set; }
        public string status { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string street { get; set; }
        public string? area { get; set; }
        public string? flatNumber { get; set; }
        public string? buildingNumber { get; set; }
        public string? image1 { get; set; }
        public string? image2 { get; set; }
        public string? image3 { get; set; }

        //need to be varcharr
        public string electricityNum { get; set; }

        public string waterNum { get; set; }

        public string gasNum { get; set; }

        public bool isDeleted { get; set; }

        // maintenance-unit 1-m(maintenance)
        public virtual List<Maintenance>? Maintenances { get; set; }

        ////community-unit 1-m(unit)
        [ForeignKey("community")]
        public int communityId { get; set; }
        public virtual Community community { get; set; }

        // bill-unit 1-m(bill)
        public virtual List<Bill> Bills  { get; set; }

        // review-unit 1-m(review)
        public virtual List<Review>? Reviews { get; set; }

        // user 1-m       (renter )
        public string? renterId { get; set; }
        public virtual ApplicationUser? renter { get; set; }

        // user 1-m       (owner )
        public string ownerId { get; set; }
        public virtual ApplicationUser owner { get; set; }

        //  unit -addvirtisement 1-1
        public virtual Addvertisement addvertisement { get; set; }

        // unit - rent 1-1
        public virtual Rent rent { get; set; }


    }
}
