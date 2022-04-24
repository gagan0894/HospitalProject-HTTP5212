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
using HospitalProject_HTTP5212.Models;
using System.Diagnostics;

namespace HospitalProject_HTTP5212.Controllers
{
    public class PatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PatientData/ListPatients
        [HttpGet]
        [ResponseType(typeof(PatientDto))]
        public IHttpActionResult ListPatients()
        {
            List<Patient> Patients = db.Patients.ToList();
            List<PatientDto> PatientDtos = new List<PatientDto>();
            DoctorDataController cont = new DoctorDataController();
            Patients.ForEach(a => {
                PatientDto patientdto = new PatientDto();
                string docName = cont.GetDoctorName(a.DoctorId);
                patientdto.PatientID = a.PatientID;
                patientdto.PatientName = a.PatientName;
                patientdto.Problem = a.Problem;
                patientdto.DoctorName = docName;
                PatientDtos.Add(patientdto);
            }); ;
            return Ok(PatientDtos);
        }

        // GET: api/DoctorData/FindDoctor/7
        [ResponseType(typeof(Patient))]
        [HttpGet]
        public IHttpActionResult FindPatient(int id)
        {
            Patient Patient = db.Patients.Find(id);
            Debug.WriteLine("Patient Name-" + Patient.PatientName);
            if (Patient == null)
            {
                return NotFound();
            }

            return Ok(Patient);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PatientID == id) > 0;
        }

        // POST: api/PatientData/UpdatePatient/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdatePatient(int id, Patient Patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Patient.PatientID)
            {
                Debug.WriteLine("ID don't match");
                return BadRequest();
            }

            db.Entry(Patient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        // POST: api/PatientData/AddPatient
        [ResponseType(typeof(Patient))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddPatient(Patient Patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(Patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Patient.PatientID }, Patient);
        }

        // POST: api/PatientData/DeletePatient/5
        [ResponseType(typeof(Patient))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient Patient = db.Patients.Find(id);
            if (Patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(Patient);
            db.SaveChanges();

            return Ok();
        }
    }
}