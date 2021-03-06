﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HacForo.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        internal void SetPassword(string password)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            var encrypPass = crypto.Compute(password);
            Password = encrypPass;
            PasswordSalt = crypto.Salt;
        }

    }

    public class UserMetaData
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "Email Address: ")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 6)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "User Name: ")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name: ")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name: ")]
        public string LastName { get; set; }

    }
}