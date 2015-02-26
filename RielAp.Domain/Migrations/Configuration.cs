namespace RielAp.Domain.Migrations
{
    using RielAp.Domain.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RielAp.Domain.Models.RealtyDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RielAp.Domain.Models.RealtyDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Profiles.AddOrUpdate(p => p.ProfileId,
                  new Profile { IsBasic = true, ProfileName="Basic", MaxAnnouncements = 10, MaxPhotosPerAnnouncements = 5, Priority = 1, Price=0, ProfileId = 1 },
                  new Profile { IsBasic = false, ProfileName = "Middle", MaxAnnouncements = 50, MaxPhotosPerAnnouncements = 8, Priority = 2, Price = 40M, ProfileId = 2 },
                  new Profile { IsBasic = false, ProfileName = "Super", MaxAnnouncements = 100, MaxPhotosPerAnnouncements = 10, Priority = 3, Price = 70M, ProfileId = 3 }
                );

            context.Roles.AddOrUpdate(r=>r.RoleName,
                new Role { RoleName = "User", HasAdminAccess = false, HasBasicAccess = true },
                  new Role { RoleName = "Admin", HasAdminAccess = true, HasBasicAccess = true }
                );
        }
    }
}
