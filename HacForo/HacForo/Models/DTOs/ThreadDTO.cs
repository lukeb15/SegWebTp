﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HacForo.Models.DTOs
{
    public class ThreadDTO
    {
        public string CreationDate { get; set; }
        public int Id { get; set; }        
        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public UserDTO User { get; set; }
        public string ImageLink { get; set; }

        public bool ValidateUser(int userId)
        {
            return User.Id == userId;
        }

        internal void UpdateModel(ForumThread threadDb)
        {
            threadDb.Description = Description;
            threadDb.ImageLink = ImageLink;
            threadDb.Title = Title;
        }
    }
}