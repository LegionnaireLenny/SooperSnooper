namespace SooperSnooper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNameLengthReAddedDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tweets", "PostDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "JoinDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "JoinDate");
            DropColumn("dbo.Tweets", "PostDate");
        }
    }
}
