using defaultASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace defaultASP.Controllers
{
    public class ConsumeApiController : Controller
    {
        // GET: ConsumeApi
        public ActionResult Index()
        {
            IEnumerable<StaffModel> staff = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:10028/api/");
                //HTTP GET
                var responseTask = client.GetAsync("RegApi");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<StaffModel>>();
                    readTask.Wait();

                    staff = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    staff = Enumerable.Empty<StaffModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(staff);
        }
        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create(StaffModel staff)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:10028/api/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<StaffModel>("RegApi", staff);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(staff);
        }
        public ActionResult Edit(int id)
        {
            StaffModel staff = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:10028/api/");
                //HTTP GET
                var responseTask = client.GetAsync("RegApi/?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<StaffModel>>();
                    readTask.Wait();

                    staff = readTask.Result[0];
                }
            }

            return View(staff);
        }
        [HttpPost]
        public ActionResult Edit(StaffModel staff)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:10028/api/");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<StaffModel>("RegApi", staff);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(staff);
        }
        public ActionResult Delete(int id = 0)
        {
            if(id <= 0)
            {
                return RedirectToAction("Index");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:10028/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("RegApi/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

    }
}