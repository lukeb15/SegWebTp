using HacForo.Mappers;
using HacForo.Models;
using HacForo.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HacForo.Controllers
{
    public class HomeController : Controller
    {
        private IMapper<ForumThread, TableThreadDTO> Mapper { set; get; }

        public HomeController()
        {
            Mapper = new TableThreadMapper();
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            using (var db = new Models.HacForoContainer())
            {
                var threads = db.ForumThreadSet.ToList();

                ViewBag.Threads = threads.ToList()
                                    .Select(ft => Mapper.MapTo(ft))
                                    .OrderByDescending(t => t.CreationDate)
                                    .ToList();
            }
                        
            return View();
        }
    }
}
