using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HacForo.Models.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 6)]
        [Display(Name = "New Password: ")]
        public string Password { get; set; }

    }
}