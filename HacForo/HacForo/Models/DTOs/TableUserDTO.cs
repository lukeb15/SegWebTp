using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HacForo.Models.DTOs
{
    public class TableUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureLink { get; set; }
    }
}