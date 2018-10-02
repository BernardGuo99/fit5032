namespace Mel_Medicare_Location_Reservation_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class role2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RoleViewModels", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RoleViewModels", "Name", c => c.String());
        }
    }
}
