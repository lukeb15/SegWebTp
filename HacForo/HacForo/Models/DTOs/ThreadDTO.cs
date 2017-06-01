using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HacForo.Models.DTOs
{
    public class ThreadDTO
    {
        public string CreationDate { get; internal set; }
        public int Id { get; internal set; }
        public string Title { get; internal set; }
        public string Description { get; internal set; }
        public UserDTO User { get; internal set; }
    }
}