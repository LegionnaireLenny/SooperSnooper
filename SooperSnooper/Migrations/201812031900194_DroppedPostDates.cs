namespace SooperSnooper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedPostDates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tweets", "User_Username", "dbo.Users");
            DropIndex("dbo.Tweets", new[] { "User_Username" });
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Tweets", "User_Username", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 15));
            AddPrimaryKey("dbo.Users", "Username");
            CreateIndex("dbo.Tweets", "User_Username");
            AddForeignKey("dbo.Tweets", "User_Username", "dbo.Users", "Username", cascadeDelete: true);
            DropColumn("dbo.Tweets", "PostDate");
            DropColumn("dbo.Users", "JoinDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "JoinDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tweets", "PostDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Tweets", "User_Username", "dbo.Users");
            DropIndex("dbo.Tweets", new[] { "User_Username" });
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Tweets", "User_Username", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Users", "Username");
            CreateIndex("dbo.Tweets", "User_Username");
            AddForeignKey("dbo.Tweets", "User_Username", "dbo.Users", "Username", cascadeDelete: true);
        }
    }
}
