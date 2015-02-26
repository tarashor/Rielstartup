namespace RielAp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Phone = c.String(),
                        EmailAddress = c.String(),
                        ReceiveNewAnnouncements = c.Boolean(nullable: false),
                        IsOwner = c.Boolean(),
                        ProfileId = c.Int(nullable: false),
                        ProfileExpires = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Profiles", t => t.ProfileId, cascadeDelete: false)
                .Index(t => t.ProfileId);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        ProfileId = c.Int(nullable: false, identity: true),
                        ProfileName = c.String(),
                        MaxAnnouncements = c.Int(nullable: false),
                        MaxPhotosPerAnnouncements = c.Int(nullable: false),
                        IsBasic = c.Boolean(nullable: false),
                        Priority = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ProfileId);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoId = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        OwnerId = c.Int(nullable: false),
                        AnnouncementId = c.Int(),
                        IsMain = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PhotoId)
                .ForeignKey("dbo.Users", t => t.OwnerId)
                .ForeignKey("dbo.Announcements", t => t.AnnouncementId, cascadeDelete: true)
                .Index(t => t.OwnerId)
                .Index(t => t.AnnouncementId);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Author = c.String(),
                        Text = c.String(),
                        Created = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MobileNumbers",
                c => new
                    {
                        MobileNumberId = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MobileNumberId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Announcements",
                c => new
                    {
                        AnnouncementID = c.Int(nullable: false, identity: true),
                        Address_AdministrativeArea = c.String(),
                        Address_SubAdministrativeArea = c.String(),
                        Address_LocalityName = c.String(),
                        Address_District = c.String(),
                        Address_Street = c.String(),
                        Address_AddressType = c.Int(nullable: false),
                        Address_Logitude = c.String(),
                        Address_Latitude = c.String(),
                        Type = c.Int(nullable: false),
                        TotalSquare_Value = c.Single(nullable: false),
                        TotalSquare_Unit = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.Int(nullable: false),
                        Notice = c.String(),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        Sold = c.Boolean(),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnnouncementID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Author = c.String(),
                        AuthorEmail = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        IsShown = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false),
                        ParentEmailId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Emails", t => t.ParentEmailId)
                .Index(t => t.ParentEmailId);
            
            CreateTable(
                "dbo.Livings",
                c => new
                    {
                        AnnouncementID = c.Int(nullable: false),
                        RoomsNumber = c.Int(nullable: false),
                        LivingSquare_Value = c.Single(nullable: false),
                        LivingSquare_Unit = c.Int(nullable: false),
                        WallMaterial = c.Int(nullable: false),
                        FloorMaterial = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnnouncementID)
                .ForeignKey("dbo.Announcements", t => t.AnnouncementID, cascadeDelete: true)
                .Index(t => t.AnnouncementID);
            
            CreateTable(
                "dbo.Apartments",
                c => new
                    {
                        AnnouncementID = c.Int(nullable: false),
                        Floor = c.Int(nullable: false),
                        MaxFloor = c.Int(nullable: false),
                        HeatingType = c.Byte(nullable: false),
                        HotWaterType = c.Byte(nullable: false),
                        PlaceForCar = c.Byte(nullable: false),
                        Balcony = c.Byte(nullable: false),
                        EntranceFrom = c.Byte(nullable: false),
                        EntranceIn = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.AnnouncementID)
                .ForeignKey("dbo.Livings", t => t.AnnouncementID, cascadeDelete: true)
                .Index(t => t.AnnouncementID);
            
            CreateTable(
                "dbo.Houses",
                c => new
                    {
                        AnnouncementID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnnouncementID)
                .ForeignKey("dbo.Livings", t => t.AnnouncementID, cascadeDelete: true)
                .Index(t => t.AnnouncementID);
            
            CreateTable(
                "dbo.Lands",
                c => new
                    {
                        AnnouncementID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnnouncementID)
                .ForeignKey("dbo.Announcements", t => t.AnnouncementID, cascadeDelete: true)
                .Index(t => t.AnnouncementID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Lands", new[] { "AnnouncementID" });
            DropIndex("dbo.Houses", new[] { "AnnouncementID" });
            DropIndex("dbo.Apartments", new[] { "AnnouncementID" });
            DropIndex("dbo.Livings", new[] { "AnnouncementID" });
            DropIndex("dbo.Emails", new[] { "ParentEmailId" });
            DropIndex("dbo.Announcements", new[] { "UserID" });
            DropIndex("dbo.MobileNumbers", new[] { "UserId" });
            DropIndex("dbo.Feedbacks", new[] { "UserId" });
            DropIndex("dbo.Photos", new[] { "AnnouncementId" });
            DropIndex("dbo.Photos", new[] { "OwnerId" });
            DropIndex("dbo.Users", new[] { "ProfileId" });
            DropForeignKey("dbo.Lands", "AnnouncementID", "dbo.Announcements");
            DropForeignKey("dbo.Houses", "AnnouncementID", "dbo.Livings");
            DropForeignKey("dbo.Apartments", "AnnouncementID", "dbo.Livings");
            DropForeignKey("dbo.Livings", "AnnouncementID", "dbo.Announcements");
            DropForeignKey("dbo.Emails", "ParentEmailId", "dbo.Emails");
            DropForeignKey("dbo.Announcements", "UserID", "dbo.Users");
            DropForeignKey("dbo.MobileNumbers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Feedbacks", "UserId", "dbo.Users");
            DropForeignKey("dbo.Photos", "AnnouncementId", "dbo.Announcements");
            DropForeignKey("dbo.Photos", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.Users", "ProfileId", "dbo.Profiles");
            DropTable("dbo.Lands");
            DropTable("dbo.Houses");
            DropTable("dbo.Apartments");
            DropTable("dbo.Livings");
            DropTable("dbo.Emails");
            DropTable("dbo.Announcements");
            DropTable("dbo.MobileNumbers");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Photos");
            DropTable("dbo.Profiles");
            DropTable("dbo.Users");
        }
    }
}
