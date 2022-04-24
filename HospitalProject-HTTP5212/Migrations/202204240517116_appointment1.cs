namespace HospitalProject_HTTP5212.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointment1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "StartTime", c => c.String());
            AlterColumn("dbo.Appointments", "EndTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "EndTime", c => c.Int(nullable: false));
            AlterColumn("dbo.Appointments", "StartTime", c => c.Int(nullable: false));
        }
    }
}
