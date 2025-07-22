using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.Models;
using System.Reflection.Metadata;

namespace Real_Estate_Project.Models
{
    public class ProjectContext: IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Addvertisement> Addvertisements { get; set; }
      
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Community> Communities { get; set; }
        public virtual DbSet<CommunityPost> CommunityPosts { get; set; }
        public virtual DbSet<Maintenance> Maintenances { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Rent> Rents { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<VerificationCode> VerificationCodes { get; set; }
        public virtual DbSet<React> Reacts { get; set; }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }

        public ProjectContext()
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

            // 1-1  owner & community
            modelBuilder.Entity<Community>()
           .HasOne(e => e.Owner)
           .WithOne(e => e.OwnerCommunity)
           .HasForeignKey<Community>(e => e.ownerId)
           .IsRequired();



            // 1-1  owner & community
            modelBuilder.Entity<ApplicationUser>()
            .HasOne(e => e.OwnerCommunity)
            .WithOne(e => e.Owner)
            .HasForeignKey<Community>(e => e.ownerId)
            .IsRequired();
        }

            
    }
}

