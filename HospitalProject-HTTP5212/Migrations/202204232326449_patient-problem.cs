namespace HospitalProject_HTTP5212.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class patientproblem : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patients", "Problem", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patients", "Problem", c => c.Int(nullable: false));
        }
    }
}
