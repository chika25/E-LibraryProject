using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystemProject.Models
{
    [Table("Books", Schema = "dbo")] // Maps to the SQL table
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; } // Primary Key

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } // Book Title

        [Required]
        [MaxLength(100)]
        public string Author { get; set; } // Author Name

        [Required]
        [MaxLength(20)]
        public string ISBN { get; set; } // ISBN (Unique)

        [Required]
        public int PublicationYear { get; set; } // Year of Publication

        [Required]
        public int CategoryID { get; set; } // Foreign Key to Category

        // Foreign Key Relationship
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; } // Navigation property for Category

        public virtual ICollection<RentalTracking> Rentals { get; set; }
        public virtual ICollection<WishListItem> WishListItems { get; set; }
    }
}
