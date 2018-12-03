namespace SooperSnooper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsernameLengthIncrease : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tweets", "User_Username", "dbo.Users");
            DropIndex("dbo.Tweets", new[] { "User_Username" });
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Tweets", "User_Username", c => c.String(nullable: false, maxLength: 16));
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 16));
            AddPrimaryKey("dbo.Users", "Username");
            CreateIndex("dbo.Tweets", "User_Username");
            AddForeignKey("dbo.Tweets", "User_Username", "dbo.Users", "Username", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tweets", "User_Username", "dbo.Users");
            DropIndex("dbo.Tweets", new[] { "User_Username" });
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Tweets", "User_Username", c => c.String(nullable: false, maxLength: 15));
            AddPrimaryKey("dbo.Users", "Username");
            CreateIndex("dbo.Tweets", "User_Username");
            AddForeignKey("dbo.Tweets", "User_Username", "dbo.Users", "Username", cascadeDelete: true);
        }
    }
}
