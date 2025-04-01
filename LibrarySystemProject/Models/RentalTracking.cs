using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystemProject.Models
{
    [Table("RentalTracking", Schema = "dbo")] // Maps to the SQL table
    public class RentalTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RentalID { get; set; } // Primary Key

        [Required]
        public int UserID { get; set; } // Foreign Key referencing User

        [Required]
        public int BookID { get; set; } // Foreign Key referencing Books

        [Required]
        public DateTime StartDate { get; set; } // Rental start date

        public DateTime? EndDate { get; set; } // Nullable - Book may not be returned yet

        public int? HistoryListID { get; set; } // Foreign Key referencing RentalHistoryList (Optional)

        // Foreign Key Relationships
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }

        [ForeignKey("HistoryListID")]
        public virtual RentalHistoryList RentalHistory { get; set; }
    }
}
