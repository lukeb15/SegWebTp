﻿using HacForo.Common;
using HacForo.Mappers;
using HacForo.Models;
using HacForo.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace HacForo.Controllers
{
    public class UserController : Controller
    {
        private IMapper<User, RegistrationDTO> RegistrationMap { set; get; }
        private IMapper<User, UserDTO>UserMap { set; get; }

        public UserController()
        {
            RegistrationMap = new RegistrationMapper();
            UserMap = new UserMapper();
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
                        string userJson = new JavaScriptSerializer().Serialize(UserMap.MapTo(user));
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
                    ModelState.AddModelError("", "Data is not correct");
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
    }
}
