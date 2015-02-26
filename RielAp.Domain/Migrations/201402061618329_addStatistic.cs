namespace RielAp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStatistic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VashmagazinApartments",
                c => new
                    {
                        VashmagazinApartmentID = c.Int(nullable: false, identity: true),
                        Address_AdministrativeArea = c.String(),
                        Address_SubAdministrativeArea = c.String(),
                        Address_LocalityName = c.String(),
                        Address_District = c.String(),
                        Address_Street = c.String(),
                        Address_AddressType = c.Int(nullable: false),
                        Address_Logitude = c.String(),
                        Address_Latitude = c.String(),
                        RoomsCount = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        TotalSquare_Value = c.Single(nullable: false),
                        TotalSquare_Unit = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Phone = c.String(),
                        Floor = c.Int(nullable: false),
                        MaxFloor = c.Int(nullable: false),
                        IsOld = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.VashmagazinApartmentID);
            
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        StatisticID = c.Int(nullable: false, identity: true),
                        City = c.String(),
                        District = c.String(),
                        RoomsCount = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        PricePerMeter = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IsOld = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StatisticID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Statistics");
            DropTable("dbo.VashmagazinApartments");
        }
    }
}
