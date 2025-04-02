using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibrarySystemProject.Models
{
    [Table("UserRoles", Schema = "dbo")]
    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; } // Primary Key


        public int UserId { get; set; }

        public int RoleId { get; set; }
    }
}