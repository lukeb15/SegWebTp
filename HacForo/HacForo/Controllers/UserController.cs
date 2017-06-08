using HacForo.Common;
using HacForo.Mappers;
using HacForo.Models;
using HacForo.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace HacForo.Controllers
{
    public class UserController : Controller
    {
        private IMapper<User, RegistrationDTO> RegistrationMap { set; get; }
        private IMapper<User, UserDTO> Mapper { set; get; }
        private IMapper<User, TableUserDTO> TableUserMap { set; get; }
        public UserController()
        {
            RegistrationMap = new RegistrationMapper();
            TableUserMap = new TableUserMapper();
            Mapper = new UserMapper();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LoginDTO loginDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = SessionManager.AuthenticateUser(loginDTO.Email, loginDTO.Password);

                    using (var db = new Models.HacForoContainer())
                    {
                        string userJson = new JavaScriptSerializer().Serialize(TableUserMap.MapTo(user));
                        string encTicket = SessionManager.GetAuthTicket(user.UserName, userJson);

                        HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                        Response.Cookies.Add(faCookie);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Login details are wrong.");
            }

            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            using (var db = new Models.HacForoContainer())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserDTO userDTO = Mapper.MapTo(db.UserSet.Find(id));
                if (userDTO == null)
                {
                    return HttpNotFound();
                }
                return View(userDTO);
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistrationDTO registrationDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new Models.HacForoContainer())
                    {
                        if (registrationDTO.IsValid(db, ModelState))
                        {
                            db.UserSet.Add(RegistrationMap.MapTo(registrationDTO));
                            db.SaveChanges();
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The data is not correct");
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            using (var db = new Models.HacForoContainer())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserDTO userDTO = Mapper.MapTo(db.UserSet.Find(id));
                if (userDTO == null)
                {
                    return HttpNotFound();
                }
                return View(userDTO);
            }
        }

        [HttpPost]
        public ActionResult Edit(RegistrationDTO registrationDTO)
        {
            try
            {
                if (!ValidateUser(registrationDTO.Id))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                using (var db = new Models.HacForoContainer())
                {
                    if (registrationDTO.IsValid(db, ModelState))
                    {
                        var userDb = db.UserSet.Find(registrationDTO.Id);

                        if (userDb == null)
                        {
                            return HttpNotFound();
                        }

                        registrationDTO.UpdateModel(userDb);
                        db.SaveChanges();

                        return View("Details", Mapper.MapTo(userDb));
                    }
                    else
                    {
                        ModelState.AddModelError("", "The data is not valid.");
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordDTO newChangePassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new Models.HacForoContainer())
                    {
                        HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                        UserDTO cookieUser = SessionManager.GetLoggedUser(authCookie);

                        var userDb = db.UserSet.Find(cookieUser.Id);

                        if (userDb == null)
                        {
                            return HttpNotFound();
                        }
                        userDb.SetPassword(newChangePassword.Password);
                        db.SaveChanges();

                        return View("Details", Mapper.MapTo(userDb));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The data is not correct");
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            // Delete the user details from cache.
            Session.Abandon();
            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();
            // Clear authentication cookie.
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }

        public bool ValidateUser(int userId)
        {
            if (userId == 0)
            {
                return false;
            }

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            UserDTO cookieUser = SessionManager.GetLoggedUser(authCookie);           

            if (cookieUser.Id == userId)
            {
                return true;
            }

            return false;
        }
    }
}
