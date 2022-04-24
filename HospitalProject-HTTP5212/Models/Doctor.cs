using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject_HTTP5212.Models
{
    public class Doctor
    {

        [Key]
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }

        public string Speciality { get; set; }

        
    }

    public class DoctorDto
    {
        public int DoctorName { get; set; }
        public string title { get; set; }

        public string description { get; set; }

        public string AuthorName { get; set; }

        public string GenreName { get; set; }


    }
}