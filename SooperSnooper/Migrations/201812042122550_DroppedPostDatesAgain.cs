namespace SooperSnooper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedPostDatesAgain : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tweets", "PostDate");
            DropColumn("dbo.Users", "JoinDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "JoinDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tweets", "PostDate", c => c.DateTime(nullable: false));
        }
    }
}
