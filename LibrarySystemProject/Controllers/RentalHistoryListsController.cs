using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibrarySystemProject.Models;

namespace LibrarySystemProject.Controllers
{
    public class RentalHistoryListsController : Controller
    {
        private LibraryContext db = new LibraryContext();

        // GET: RentalHistoryLists

        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login"); // Redirect to login if not logged in
            }

            int userId = (int)Session["UserID"];

            var rentedBooks = db.RentalTracking
                                .Where(r => r.RentalHistory.UserID == userId)
                                .Include(r => r.Book) // Load Book details
                                .ToList();

            return View(rentedBooks);

         
        }

        // GET: RentalHistoryLists/Details/5
        public ActionResult Details(int id)
        {
            var rentalHistoryItem = db.RentalTracking
                                  .Include(w => w.Book)
                                  .FirstOrDefault(w => w.BookID == id);  // Assuming BookID is the foreign key to the Book

            if (rentalHistoryItem == null)
            {
                return HttpNotFound();
            }

            return View(rentalHistoryItem.Book);  // Pass the Book to the view
        }

    }
}
