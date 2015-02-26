using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace RielAp.Domain.Models
{
    public class RealtyDBContext : DbContext
    {
        static RealtyDBContext()
        {
            Database.SetInitializer<RealtyDBContext>(null);
        }

		public RealtyDBContext()
			: base()
		{
		}

        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<VashmagazinApartment> VashmagazinApartments { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<MobileNumber> MobileNumbers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LivingAnnouncement>().ToTable("Livings");
            modelBuilder.Entity<ApartmentAnnouncement>().ToTable("Apartments");
            modelBuilder.Entity<HouseAnnouncement>().ToTable("Houses");
            modelBuilder.Entity<LandAnnouncement>().ToTable("Lands");


        }

        
    }
}
