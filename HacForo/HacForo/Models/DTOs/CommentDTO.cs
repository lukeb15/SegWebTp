using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HacForo.Models.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public TableUserDTO User { get; set; }
        [AllowHtml]
        public string Message { get; set; }
        public string CreationDate { get; set; }
        public int ThreadId { get; set; }
    }
}