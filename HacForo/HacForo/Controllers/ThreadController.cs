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

namespace HacForo.Controllers
{
    public class ThreadController : Controller
    {
        private HacForoContainer db = new HacForoContainer();
        private IMapper<ForumThread, ThreadDTO> Mapper { set; get; }

        public ThreadController()
        {
            Mapper = new ThreadMapper();
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
            return View(threadDTO);
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
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                UserDTO serializeModel = serializer.Deserialize<UserDTO>(authTicket.UserData);                

                if (serializeModel != null)
                {
                    if (ModelState.IsValid)
                    {
                        thread.User = serializeModel;
                        db.ForumThreadSet.Add(Mapper.MapTo(thread));
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }

                    return View(thread);
                }                    
            }
            throw new UnauthorizedAccessException("You should be logged to create threads");
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
