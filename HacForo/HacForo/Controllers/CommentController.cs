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
using System.Web.Security;
using System.Web.Script.Serialization;

namespace HacForo.Controllers
{
    public class CommentController : Controller
    {
        private HacForoContainer db = new HacForoContainer();
        private IMapper<ForumThread, ThreadDTO> ThreadMap { set; get; }
        private IMapper<Comment, CommentDTO> Mapper { set; get; }

        public CommentController()
        {
            ThreadMap = new ThreadMapper();
            Mapper = new CommentMapper();
        }

        // GET: ThreadDTO/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentDTO commentDTO = Mapper.MapTo(db.CommentSet.Find(id));

            if (commentDTO == null)
            {
                return HttpNotFound();
            }

            return View(commentDTO);
        }

        // GET: CommentDTO/Create/{threadId}
        public ActionResult Create(int? threadId)
        {
            if(!threadId.HasValue || threadId < 1)
            {
                throw new UnauthorizedAccessException("The threadId is invalid");
            }

            try
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    TableUserDTO serializeModel = serializer.Deserialize<TableUserDTO>(authTicket.UserData);
                    CommentDTO comment = new CommentDTO();

                    if (serializeModel != null)
                    {
                        if (ModelState.IsValid)
                        {
                            comment.User = serializeModel;
                            comment.ThreadId = threadId.Value;

                            return View(comment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            throw new UnauthorizedAccessException("You must be logged to create threads");
        }

        // POST: CommentDTO/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(CommentDTO comment)
        {
            if (comment.ThreadId < 1)
            {
                throw new UnauthorizedAccessException("The threadId is invalid");
            }

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
                            comment.User = serializeModel;

                            db.CommentSet.Add(Mapper.MapTo(comment));
                            db.SaveChanges();

                            return RedirectToAction("Details", "Thread", ThreadMap.MapTo(
                                    db.ForumThreadSet
                                          .Include(t => t.Comments.Select(c => c.User))
                                        .FirstOrDefault(t => t.Id == comment.ThreadId)
                                    ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            throw new UnauthorizedAccessException("You must be logged to create threads");
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
