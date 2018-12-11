namespace SooperSnooper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTweetPostDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tweets", "PostDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tweets", "PostDate");
        }
    }
}
