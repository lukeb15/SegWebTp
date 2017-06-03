using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HacForo.Models.DTOs
{
    public class RegistrationDTO
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

        [Required]
        [Display(Name = "Profile Picture Link: ")]
        public string ProfilePictureLink { get; set; }

        public bool IsValid(HacForoContainer db, ModelStateDictionary modelState)
        {
            bool isValid = true;
            if (db.UserSet.Where(u => u.Email == Email).Count() != 0)
            {
                modelState.AddModelError("Email", "There is already a User with that Email");
                isValid = false;
            }
            if (db.UserSet.Where(u => u.UserName == UserName).Count() != 0)
            {
                modelState.AddModelError("UserName", "There is already a User with that User Name");
                isValid = false;
            }

            return isValid;
        }
    }
}