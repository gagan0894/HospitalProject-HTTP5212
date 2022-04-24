using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using HospitalProject_HTTP5212.Models;
using System.Web.Script.Serialization;
using System.Dynamic;

namespace HospitalProject_HTTP5212.Controllers
{
    public class PatientController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PatientController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:44308/api/");
        }

        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Patient/ListPatients
        public ActionResult ListPatients()
        {
            //objective: communicate with our Species data api to retrieve a list of Speciess
            //curl http://localhost:44308/api/Patientdata/listPatients


            string url = "Patientdata/listPatients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<PatientDto> Patients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;
            //Debug.WriteLine("Number of Speciess received : ");
            //Debug.WriteLine(Speciess.Count());


            return View(Patients);
        }


        [Authorize]
        // GET: Patient/New
        public ActionResult New()
        {
            GetApplicationCookie();
            string url = "doctordata/listdoctors";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Doctor> DoctorsOptions = response.Content.ReadAsAsync<IEnumerable<Doctor>>().Result;

            return View(DoctorsOptions);
        }

        // POST: Patient/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Patient Patient)
        {
            GetApplicationCookie();
            Debug.WriteLine("the json payload is :"+ Patient);
            //Debug.WriteLine(Species.SpeciesName);
            //objective: add a new Species into our system using the API
            //curl -H "Content-Type:application/json" -d @Species.json https://localhost:44308/api/Doctordata/addDoctor 
            string url = "Patientdata/addPatient";

           
            string jsonpayload = jss.Serialize(Patient);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListPatients");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }
        [Authorize]
        // GET: Patient/Update/5
        public ActionResult Update(int id)
        {
            GetApplicationCookie();
            string url = "Patientdata/findPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Patient patient = response.Content.ReadAsAsync<Patient>().Result;

            string url1 = "doctordata/listdoctors";
            HttpResponseMessage response1 = client.GetAsync(url1).Result;
            IEnumerable<Doctor> DoctorsOptions = response1.Content.ReadAsAsync<IEnumerable<Doctor>>().Result;
            dynamic mymodel = new ExpandoObject();
            mymodel.Doctors = DoctorsOptions;
            mymodel.Patient = patient;
            return View(mymodel);
        }

        // POST: Species/Modify/5
        [HttpPost]
        [Authorize]
        public ActionResult Modify(int id, Patient patient)
        {
            GetApplicationCookie();
            string url = "patientdata/updatepatient/" + id;
            patient.PatientID = id;
            string jsonpayload = jss.Serialize(patient);
            Debug.WriteLine("JSON-" + jsonpayload);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListPatients");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        [Authorize]
        // GET: Patient/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();
            string url = "patientdata/findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Patient patient = response.Content.ReadAsAsync<Patient>().Result;
            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "patientdata/deletepatient/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListPatients");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


    }
}