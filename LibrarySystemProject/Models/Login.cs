using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystemProject.Models
{
    
    public class Login
    {
        [Required(ErrorMessage ="Please enter your Login ID (email address)")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage ="Your email address is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(255)]
        public string Password { get; set; }

    }
}
