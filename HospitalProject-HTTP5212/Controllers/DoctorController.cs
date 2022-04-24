using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using HospitalProject_HTTP5212.Models;
using System.Web.Script.Serialization;

namespace HospitalProject_HTTP5212.Controllers
{
    public class DoctorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DoctorController()
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

        // GET: Doctor/ListDoctors
        public ActionResult ListDoctors()
        {
            //objective: communicate with our Species data api to retrieve a list of Speciess
            //curl http://localhost:44308/api/Doctordata/listdoctors


            string url = "doctordata/listdoctors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<Doctor> Doctors = response.Content.ReadAsAsync<IEnumerable<Doctor>>().Result;
            //Debug.WriteLine("Number of Speciess received : ");
            //Debug.WriteLine(Speciess.Count());


            return View(Doctors);
        }

       
        [Authorize]
        // GET: Doctor/New
        public ActionResult New()
        {
            GetApplicationCookie();
            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Doctor Doctor)
        {
            GetApplicationCookie();
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Species.SpeciesName);
            //objective: add a new Species into our system using the API
            //curl -H "Content-Type:application/json" -d @Species.json https://localhost:44308/api/Doctordata/addDoctor 
            string url = "doctordata/adddoctor";


            string jsonpayload = jss.Serialize(Doctor);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListDoctors");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }
        [Authorize]
        // GET: Doctor/Update/5
        public ActionResult Update(int id)
        {
            GetApplicationCookie();
            string url = "doctordata/finddoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Doctor doctor = response.Content.ReadAsAsync<Doctor>().Result;
            Debug.WriteLine("Doctor name-" + doctor.DoctorName);
            Debug.WriteLine("Doctor Speciality-" + doctor.Speciality);
            return View(doctor);
        }

        // POST: Species/Modify/5
        [HttpPost]
        [Authorize]
        public ActionResult Modify(int id, Doctor doctor)
        {
            GetApplicationCookie();
            string url = "doctordata/updatedoctor/" + id;
            doctor.DoctorID = id;
            string jsonpayload = jss.Serialize(doctor);
            Debug.WriteLine("JSON-" + jsonpayload);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListDoctors");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        [Authorize]
        // GET: Species/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();
            string url = "doctordata/finddoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Doctor doctor = response.Content.ReadAsAsync<Doctor>().Result;
            return View(doctor);
        }

        // POST: Doctor/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "doctordata/deletedoctor/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListDoctors");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}