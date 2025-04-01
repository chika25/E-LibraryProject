using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LibrarySystemProject.Models
{
    public class LibraryContext : DbContext
    {
        public LibraryContext() : base("LibraryDBConnection") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RentalHistoryList> RentalHistoryList { get; set; }
        public DbSet<RentalTracking> RentalTracking { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<WishListItem> WishListItem { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Defining the relationship between WishList and User
            modelBuilder.Entity<WishList>()
                        .HasRequired(w => w.User)  // A WishList must have a User
                        .WithMany(u => u.WishLists)  // A User can have many WishLists
                        .HasForeignKey(w => w.UserID);  // The foreign key property

            base.OnModelCreating(modelBuilder);
        }
    }
}
