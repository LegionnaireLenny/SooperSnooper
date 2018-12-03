namespace SooperSnooper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTwitterModel : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Tweets", name: "User_Username", newName: "Username");
            RenameIndex(table: "dbo.Tweets", name: "IX_User_Username", newName: "IX_Username");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Tweets", name: "IX_Username", newName: "IX_User_Username");
            RenameColumn(table: "dbo.Tweets", name: "Username", newName: "User_Username");
        }
    }
}
