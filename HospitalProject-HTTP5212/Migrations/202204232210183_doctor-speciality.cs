namespace HospitalProject_HTTP5212.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doctorspeciality : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Doctors", "Speciality", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Doctors", "Speciality", c => c.Int(nullable: false));
        }
    }
}
