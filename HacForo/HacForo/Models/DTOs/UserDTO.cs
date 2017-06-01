using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HacForo.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; internal set; }
        public string UserName { get; internal set; }
    }
}