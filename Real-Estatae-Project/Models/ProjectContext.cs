using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.Models;

namespace Real_Estate_Project.Models
{
    public class ProjectContext: IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Addvertisement> Addvertisements { get; set; }
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<BillParent> BillParents { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Community> Communities { get; set; }
        public virtual DbSet<CommunityPost> CommunityPosts { get; set; }
        public virtual DbSet<Maintenance> Maintenances { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Rent> Rents { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<VerificationCode> VerificationCodes { get; set; }
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // owner relationship
            modelBuilder.Entity<Unit>()
                .HasOne(o => o.owner)
                .WithMany(u => u.OwnerUnits)
                .HasForeignKey(o => o.ownerId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascading delete if needed

            // renter relationship
            modelBuilder.Entity<Unit>()
                .HasOne(o => o.renter)
                .WithMany(u => u.RenterUnits)
                .HasForeignKey(o => o.renterId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
