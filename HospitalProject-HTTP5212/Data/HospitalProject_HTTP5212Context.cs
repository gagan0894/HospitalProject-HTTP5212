using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HospitalProject_HTTP5212.Data
{
    public class HospitalProject_HTTP5212Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public HospitalProject_HTTP5212Context() : base("name=HospitalProject_HTTP5212Context")
        {
        }

        public System.Data.Entity.DbSet<HospitalProject_HTTP5212.Models.Doctor> Doctors { get; set; }
    }
}
