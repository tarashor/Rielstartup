namespace RielAp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleName = c.String(nullable: false, maxLength: 128),
                        HasAdminAccess = c.Boolean(nullable: false),
                        HasBasicAccess = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RoleName);
            
            AddColumn("dbo.Users", "TemporaryPassword", c => c.String());
            AddColumn("dbo.Users", "TemporaryPasswordExpires", c => c.DateTime());
            AddColumn("dbo.Users", "RegisterDate", c => c.DateTime());
            AddColumn("dbo.Users", "RoleName", c => c.String(maxLength: 128));
            AddForeignKey("dbo.Users", "RoleName", "dbo.Roles", "RoleName");
            CreateIndex("dbo.Users", "RoleName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "RoleName" });
            DropForeignKey("dbo.Users", "RoleName", "dbo.Roles");
            DropColumn("dbo.Users", "RoleName");
            DropColumn("dbo.Users", "RegisterDate");
            DropColumn("dbo.Users", "TemporaryPasswordExpires");
            DropColumn("dbo.Users", "TemporaryPassword");
            DropTable("dbo.Roles");
        }
    }
}
