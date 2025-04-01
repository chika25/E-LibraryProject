using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace LibrarySystemProject.Models
{
    [Table("User", Schema = "dbo")] 
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; } 

        [Required(ErrorMessage = "Please enter your first name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } 

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage ="Please confirm your password")]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public virtual ICollection<RentalTracking> Rentals { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
    }
}
