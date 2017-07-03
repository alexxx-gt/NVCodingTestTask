using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using NVCodingTestTask.Models;

namespace NVCodingTestTask.Controllers
{
    public class HomeController : Controller
    {
        private NVCodingTestTask.Models.UserContext db = new NVCodingTestTask.Models.UserContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ListUsers()
        {
            return View(db.Users);
        }

        public ActionResult EditUser(int id)
        {
            User user = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return Redirect("/Home/ListUsers");
            }
        }

        public ActionResult CreateUser()
        {
            return View("EditUser");
        }

        [HttpPost]
        public JsonResult AjaxSaveData(string userName, string email, string avatar, string skypeLogin, string signature, string id)
        {
            int identity = -1;
            Int32.TryParse(id, out identity);

            
            if (ModelState.IsValid)
            {
                if (identity == 0)
                {
                    User user = new User()
                    {
                        Name = userName,
                        Email = email,
                        Avatar = avatar,
                        SkypeLogin = skypeLogin,
                        Signature = signature
                    };

                    db.Users.Add(user);
                    db.SaveChanges();

                    //getting last added entry to for sending id to the view for further requests
                    User justAddedUser = db.Users.ToList().LastOrDefault();

                    //operation successfull - sending success flag and last added entry id for further requests
                    return Json(new {success = "true", justAddedId = justAddedUser.Id.ToString()});
                }
                else
                {
                    User userForEdit = db.Users.Where(m => m.Id == identity).FirstOrDefault();

                    if (userForEdit != null)
                    {
                        userForEdit.Email = email;
                        userForEdit.Name = userName;
                        userForEdit.Avatar = avatar;
                        userForEdit.SkypeLogin = skypeLogin;
                        userForEdit.Signature = signature;

                        db.SaveChanges();

                        //we already have an id - so just added id will be -1, process this value in script
                        return Json(new { success = "true", justAddedId = "-1" });
                    }

                    //object with id from page was not found in database - something went wrong
                    return Json(new { success = "false" });
                }
            }
            else
            {
                //Model invalid, something went wrong
                return Json(new {success = "false"});
            }
        }
    }
}