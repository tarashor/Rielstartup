namespace RielAp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTypeToLand : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lands", "ApplyType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lands", "ApplyType");
        }
    }
}
