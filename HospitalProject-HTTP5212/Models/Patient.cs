using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject_HTTP5212.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }
        public string PatientName { get; set; }

        public string Problem { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
    }


    public class PatientDto
    {
        public int PatientID { get; set; }
        public string PatientName { get; set; }

        public string Problem { get; set; }
        public string DoctorName { get; set; }
    }

}