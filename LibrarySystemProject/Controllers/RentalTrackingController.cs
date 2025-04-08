using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibrarySystemProject.Models;

namespace LibrarySystemProject.Controllers
{
    public class RentalTrackingController : Controller
    {
        private LibraryContext db = new LibraryContext();
        // GET: RentalTracking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RentBook(int bookId)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            int userId = (int)Session["UserID"];

            // Prevent duplicate rentals for the same book if it's not returned yet
            bool alreadyRented = db.RentalTracking.Any(r =>
                r.UserID == userId &&
                r.BookID == bookId &&
                r.EndDate == null);

            if (alreadyRented)
            {
                TempData["Message"] = "You have already rented this book and not yet returned it.";
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            var rental = new RentalTracking
            {
                UserID = userId,
                BookID = bookId,
                StartDate = DateTime.Now,
                EndDate = null // Not returned yet
            };

            db.RentalTracking.Add(rental);
            db.SaveChanges();

            TempData["Message"] = "Book rented successfully!";
            return RedirectToAction("Details", "Books", new { id = bookId });
            //return RedirectToAction("Index", "RentalHistoryLists");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnBook(int rentalId)
        {
            var rental = db.RentalTracking.FirstOrDefault(r => r.RentalID == rentalId);

            if (rental == null || rental.EndDate != null)
            {
                TempData["Message"] = "This book is already returned or rental not found.";
                return RedirectToAction("Index", "RentalHistoryLists");
            }

            rental.EndDate = DateTime.Now;
            db.SaveChanges();

            TempData["Message"] = "Book returned successfully!";
            return RedirectToAction("Index", "RentalHistoryLists");
        }

    }
}