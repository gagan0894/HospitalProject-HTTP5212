using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalProject_HTTP5212.Data;
using HospitalProject_HTTP5212.Models;
using System.Diagnostics;

namespace HospitalProject_HTTP5212.Controllers
{
    public class DoctorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DoctorData/ListDoctors
        [HttpGet]
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult ListDoctors()
        {
            List<Doctor> Doctors = db.Doctors.ToList();

            return Ok(Doctors);
        }

        // GET: api/DoctorData/FindDoctor/7
        [ResponseType(typeof(Doctor))]
        [HttpGet]
        public IHttpActionResult FindDoctor(int id)
        {
            Doctor Doctor = db.Doctors.Find(id);
            Debug.WriteLine("DoctorID-"+id);
            Debug.WriteLine("Doctor Name-" + Doctor.DoctorName);
            if (Doctor == null)
            {
                return NotFound();
            }

            return Ok(Doctor);
        }
        public string GetDoctorName(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            Debug.WriteLine("Doctor-" + doctor.DoctorName);
            if (doctor == null)
            {
                return null;
            }

            return doctor.DoctorName;
        }
        private bool DoctorExists(int id)
        {
            return db.Doctors.Count(e => e.DoctorID == id) > 0;
        }

        // POST: api/DoctorData/UpdateDoctor/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateDoctor(int id, Doctor Doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Doctor.DoctorID)
            {
                Debug.WriteLine("ID don't match");
                return BadRequest();
            }

            db.Entry(Doctor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DoctorData/AddDoctor
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Doctors.Add(doctor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctor.DoctorID }, doctor);
        }

        // POST: api/DoctorData/DeleteDoctor/5
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteDoctor(int id)
        {
            Doctor Doctor = db.Doctors.Find(id);
            if (Doctor == null)
            {
                return NotFound();
            }

            db.Doctors.Remove(Doctor);
            db.SaveChanges();

            return Ok();
        }
    }
}