using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystemProject.Models

{
    [Table("RentalHistoryList", Schema = "dbo")] // Mapping to the database table
    public class RentalHistoryList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HistoryListID { get; set; } // Primary Key

        [Required]
        public int UserID { get; set; } // Foreign Key (Unique)

        [ForeignKey("UserID")]
        public virtual User User { get; set; } // Navigation Property
    }
}
