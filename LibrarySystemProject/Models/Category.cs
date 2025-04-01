using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystemProject.Models
{
    [Table("Category", Schema = "dbo")] // Maps to the SQL table
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        [Index(IsUnique = true)] // Ensures CategoryName is unique
        public string CategoryName { get; set; } // Name of the Category
        public List<Book> Books { get; set; }
    }
}
