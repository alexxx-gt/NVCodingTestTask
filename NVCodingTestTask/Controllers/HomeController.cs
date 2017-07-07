using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using NVCodingTestTask.Models;
using NVCodingTestTask.ViewModels;

namespace NVCodingTestTask.Controllers
{
    public class HomeController : Controller
    {
        private NVCodingTestTask.Models.UserContext db = new NVCodingTestTask.Models.UserContext();
        private UnitOfWork unitOfWork;

        public HomeController()
        {
            unitOfWork = new UnitOfWork();
        }

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
            //Configuring mapper
            Mapper.Initialize(cfg => cfg.CreateMap<User, UserViewModel>());

            //Mapping
            var users = Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(unitOfWork.Users.ListUsers());
            //var users = unitOfWork.Users.ListUsers();

            return View(users);
        }

        public ActionResult EditUser(int id)
        {
            //User user = db.Users.Where(u => u.Id == id).FirstOrDefault();
            Mapper.Initialize(cfg => cfg.CreateMap<User, UserViewModel>());

            var user = Mapper.Map<User, UserViewModel>(unitOfWork.Users.GetUser(id));
            //User user = unitOfWork.Users.GetUser(id);

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
        public JsonResult AjaxSaveData(string userName, string email, string skypeLogin, string signature, string id)
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
                        //Avatar = avatar,
                        SkypeLogin = skypeLogin,
                        Signature = signature
                    };

                    unitOfWork.Users.Create(user);
                    unitOfWork.Save();


                    //db.Users.Add(user);
                    //db.SaveChanges();

                    //getting last added entry to for sending id to the view for further requests
                    //User justAddedUser = db.Users.ToList().LastOrDefault();
                    User justAddedUser = unitOfWork.Users.GetLast();

                    //operation successfull - sending success flag and last added entry id for further requests
                    return Json(new { success = "true", justAddedId = justAddedUser.Id.ToString() });
                }
                else
                {
                    //User userForEdit = db.Users.Where(m => m.Id == identity).FirstOrDefault();
                    User userForEdit = unitOfWork.Users.GetUser(identity);

                    if (userForEdit != null)
                    {
                        userForEdit.Email = email;
                        userForEdit.Name = userName;
                        //userForEdit.Avatar = avatar;
                        userForEdit.SkypeLogin = skypeLogin;
                        userForEdit.Signature = signature;

                        //db.SaveChanges();

                        unitOfWork.Save();

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
                return Json(new { success = "false" });
            }
        }

        [HttpPost]
        public JsonResult AjaxSaveImage(int id)
        {
            string path = string.Empty;

            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];

                if (upload != null)
                {
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    upload.SaveAs(Server.MapPath("/Files/avatar_id_" + id + fileName.Substring(fileName.LastIndexOf("."))));
                    path = "/Files/avatar_id_" + id + fileName.Substring(fileName.LastIndexOf("."));

                    //string fileName = System.IO.Path.GetFileName(upload.FileName);
                    //upload.SaveAs(Server.MapPath("/Files/avatar_id_" + fileName));
                    //path = "/Files/avatar_id_" + fileName;
                }
            }

            User user = unitOfWork.Users.GetUser(id);

            if (user != null)
            {
                user.Avatar = path;

                try
                {
                    unitOfWork.Save();
                }
                catch (DbEntityValidationException ex)
                {
                    var enumerator = ex.EntityValidationErrors.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        var en2 = enumerator.Current.ValidationErrors.GetEnumerator();

                        while (en2.MoveNext())
                        {
                            ModelState.AddModelError(en2.Current.PropertyName, en2.Current.ErrorMessage);
                        }

                    }
                }
            }

            return Json(new { success = "true" });
        }
    }
}