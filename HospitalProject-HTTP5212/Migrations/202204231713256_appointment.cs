namespace HospitalProject_HTTP5212.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentID = c.Int(nullable: false, identity: true),
                        AppointmentData = c.String(),
                        StartTime = c.Int(nullable: false),
                        EndTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppointmentID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Appointments");
        }
    }
}
