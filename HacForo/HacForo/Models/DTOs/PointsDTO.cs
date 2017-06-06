using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HacForo.Models.DTOs
{
    public class PointsDTO
    {
        public int Points { get; set; }
        public int ThreadId { get; set; }
    }
}