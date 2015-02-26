namespace RielAp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsVisiblePhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Photos", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.Announcements", "IsVisible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcements", "IsVisible");
            DropColumn("dbo.Photos", "IsVisible");
            DropColumn("dbo.Photos", "Created");
        }
    }
}
