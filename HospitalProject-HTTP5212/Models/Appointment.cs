using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject_HTTP5212.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }
        public string AppointmentData { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }


    }
}