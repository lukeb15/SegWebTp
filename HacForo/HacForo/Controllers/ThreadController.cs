using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HacForo.Models;
using HacForo.Models.DTOs;
using HacForo.Mappers;
using System;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;
using System.Data.Entity;

namespace HacForo.Controllers
{
    public class ThreadController : Controller
    {
        private HacForoContainer db = new HacForoContainer();
        private IMapper<ForumThread, ThreadDTO> Mapper { set; get; }
        private IMapper<ForumThread, TableThreadDTO> TableMapper { set; get; }
        private IMapper<User, TableUserDTO> TableUserMap { set; get; }

        public ThreadController()
        {
            Mapper = new ThreadMapper();
            TableMapper = new TableThreadMapper();
        }

        // GET: ThreadDTO
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.ForumThreadSet.ToList()
                        .Select(t => Mapper.MapTo(t))
                        .ToList());
        }

        // GET: ThreadDTO/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThreadDTO threadDTO = Mapper.MapTo(db.ForumThreadSet.Find(id));
            if (threadDTO == null)
            {
                return HttpNotFound();
            }

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                UserDTO serializeModel = serializer.Deserialize<UserDTO>(authTicket.UserData);

                threadDTO.CheckUserCanPoint(serializeModel.Id);
            }

            return View(threadDTO);
        }

        // POST: ThreadDTO/Edit/5
        [HttpPost]
        public ActionResult Edit(ThreadDTO thread)
        {
            if (!ValidateThread(thread.Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var threadDb = db.ForumThreadSet.Find(thread.Id);

            if (threadDb == null)
            {
                return HttpNotFound();
            }

            thread.UpdateModel(threadDb);
            db.SaveChanges();

            return View("Details", Mapper.MapTo(threadDb));
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!(id.HasValue && ValidateThread(id.Value)))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var threadDb = db.ForumThreadSet.Find(id);

            if (threadDb == null)
            {
                return HttpNotFound();
            }

            return View(Mapper.MapTo(threadDb));
        }

        // GET: ThreadDTO/Create
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (!(id.HasValue && ValidateThread(id.Value)))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ForumThread threadDb = db.ForumThreadSet.Find(id.Value);

            while(threadDb.UserThreadPoints.Count > 0)
            {
                var utp =threadDb.UserThreadPoints.First();
                threadDb.UserThreadPoints.Remove(utp);
                db.Entry(utp).State = EntityState.Deleted;
            }

            while (threadDb.Comments.Count > 0)
            {
                var c = threadDb.Comments.First();
                threadDb.Comments.Remove(c);
                db.Entry(c).State = EntityState.Deleted;
            }

            if (threadDb != null)
                db.ForumThreadSet.Attach(threadDb);
            else
                threadDb = new ForumThread { Id = id.Value };

            db.Entry(threadDb).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public bool ValidateThread(int threadId)
        {
            if (threadId == 0)
            {
                return false;
            }

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                UserDTO cookieUser = serializer.Deserialize<UserDTO>(authTicket.UserData);

                if (cookieUser != null)
                {
                    var threadDb = db.ForumThreadSet.Find(threadId);
                    if (cookieUser.Id == threadDb.UserId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // GET: ThreadDTO/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ThreadDTO/Create
        [HttpPost]
        public ActionResult Create(ThreadDTO thread)
        {
            try
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    TableUserDTO serializeModel = serializer.Deserialize<TableUserDTO>(authTicket.UserData);

                    if (serializeModel != null)
                    {
                        if (ModelState.IsValid)
                        {
                            thread.User = serializeModel;
                            db.ForumThreadSet.Add(Mapper.MapTo(thread));
                            db.SaveChanges();

                            return RedirectToAction("Index", "Home");
                        }

                        return View(thread);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            throw new UnauthorizedAccessException("You must be logged to create threads");
        }


        [HttpGet]
        public ActionResult Points(int points, int threadId)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            ThreadDTO threadDTO = Mapper.MapTo(db.ForumThreadSet.Find(threadId));
            
            if (threadDTO == null)
            {
                ModelState.AddModelError("", "The Thread does not exist.");
            }
            if(1 > points || points > 10)
            {
                ModelState.AddModelError("", "The Points are invalid.");
            }

            if (ModelState.IsValid)
            {
                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    UserDTO serializeModel = serializer.Deserialize<UserDTO>(authTicket.UserData);

                    if (serializeModel != null)
                    {
                        if (serializeModel.Id == threadDTO.User.Id)
                        {
                            ModelState.AddModelError("", "You can't add points to your own thread.");
                        }
                        if (db.UserThreadPointsSet.Where(utp => utp.UserId == serializeModel.Id && utp.ThreadId == threadId).Count() > 0)
                        {
                            ModelState.AddModelError("", "You have already pointed this thread");
                        }

                        if (ModelState.IsValid)
                        {
                            db.UserThreadPointsSet.Add(new UserThreadPoints
                            {
                                UserId = serializeModel.Id,
                                Points = points,
                                ThreadId = threadId
                            });

                            threadDTO.Points += points;

                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException("You must be logged to create threads");
                }
            }

            return RedirectToAction("Details", new { id = threadDTO.Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}