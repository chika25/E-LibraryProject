using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystemProject.Models
{
    [Table("WishList", Schema = "dbo")] // Maps to the SQL table
    public class WishList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WishListID { get; set; } // Primary Key

        [Required]
        public int UserID { get; set; } // Foreign Key from User Table

        // Navigation property to User
        public virtual User User { get; set; } // Optional navigation property to User model

        // Navigation property for related WishListItems
        public virtual ICollection<WishListItem> WishListItems { get; set; }
    }
}
