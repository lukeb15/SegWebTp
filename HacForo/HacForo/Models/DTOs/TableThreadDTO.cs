using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HacForo.Models.DTOs
{
    public class TableThreadDTO
    {
        public string CreationDate { get; set; }
        public int Id { get; set; }        
        public string Title { get; set; }
        public TableUserDTO User { get; set; }
        public string ImageLink { get; set; }
    }
}