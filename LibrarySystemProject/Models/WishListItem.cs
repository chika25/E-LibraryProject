using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystemProject.Models
{
    [Table("WishListItem", Schema = "dbo")] // Maps to the SQL table
    public class WishListItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WishListItemID { get; set; } // Primary Key

        [Required]
        public int WishListID { get; set; } // Foreign Key from WishList Table

        [Required]
        public int BookID { get; set; } // Foreign Key from Books Table

        // Navigation property for the related WishList
        [ForeignKey("WishListID")]
        public virtual WishList WishList { get; set; }

        // Navigation property for the related Book
        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }
    }
}
