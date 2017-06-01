using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HacForo.Models;
using HacForo.Models.DTOs;
using HacForo.Mappers;

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
        public ActionResult Index()
        {
            return View(db.ForumThreadSet
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: ThreadDTO/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CreationDate,Title")] ThreadDTO threadDTO)
        {
            if (ModelState.IsValid)
            {
                db.ForumThreadSet.Add(Mapper.MapTo(threadDTO));
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(threadDTO);
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
