namespace RielAp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserConfirmation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "ConfirmationCode", c => c.Guid(nullable: false));
            DropColumn("dbo.Users", "IsOwner");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "IsOwner", c => c.Boolean());
            DropColumn("dbo.Users", "ConfirmationCode");
            DropColumn("dbo.Users", "IsConfirmed");
        }
    }
}
