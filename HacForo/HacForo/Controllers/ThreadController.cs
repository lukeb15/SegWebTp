using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HacForo.Models;
using HacForo.Models.DTOs;
using HacForo.Mappers;
using System;

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
            if (thread.User != null)
            {
                if (ModelState.IsValid)
                {
                    //TODO: pass the user id and set it there
                    thread.User = new UserDTO { Id = db.UserSet.First().Id };

                    db.ForumThreadSet.Add(Mapper.MapTo(thread));
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(thread);
            }
            else
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
