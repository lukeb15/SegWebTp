using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HacForo.Models.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public UserDTO User { get; set; }
        public string Message { get; set; }
        public string CreationDate { get; set; }
        public int ThreadId { get; set; }
    }
}