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

            return View(users);
        }

        public ActionResult EditUser(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<User, UserViewModel>());

            var user = Mapper.Map<User, UserViewModel>(unitOfWork.Users.GetUser(id));

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
                        SkypeLogin = skypeLogin,
                        Signature = signature
                    };

                    unitOfWork.Users.Create(user);
                    unitOfWork.Save();

                    //getting last added entry to for sending id to the view for further requests
                    User justAddedUser = unitOfWork.Users.GetLast();

                    //operation successfull - sending success flag and last added entry id for further requests
                    return Json(new {success = "true", justAddedId = justAddedUser.Id.ToString()});
                }
                else
                {
                    User userForEdit = unitOfWork.Users.GetUser(identity);

                    if (userForEdit != null)
                    {
                        userForEdit.Email = email;
                        userForEdit.Name = userName;
                        userForEdit.SkypeLogin = skypeLogin;
                        userForEdit.Signature = signature;

                        unitOfWork.Save();

                        //we already have an id - so just added id will be -1, process this value in script
                        return Json(new {success = "true", justAddedId = "-1"});
                    }

                    //object with id from page was not found in database - something went wrong
                    return Json(new {success = "false"});
                }
            }
            else
            {
                //Model invalid, something went wrong
                return Json(new {success = "false"});
            }
        }

        [HttpPost]
        public JsonResult AjaxSaveImage(int id)
        {
            string path = string.Empty;

            string file = Request.Files.GetKey(0);

            var upload = Request.Files[file];

            if (upload != null)
            {
                //renaming and saving on disk
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                upload.SaveAs(Server.MapPath("/Files/avatar_id_" + id + fileName.Substring(fileName.LastIndexOf("."))));
                path = "/Files/avatar_id_" + id + fileName.Substring(fileName.LastIndexOf("."));
            }

            User user = unitOfWork.Users.GetUser(id);

            if (user != null)
            {
                user.Avatar = path;

                unitOfWork.Save();
            }

            return Json(new {success = "true"});

        }
    }
}