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
        public ActionResult RentBook(int bookId, DateTime StartDate, DateTime EndDate)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            int userId = (int)Session["UserID"];

            // Prevent duplicate rentals for the same book if it's not returned yet
            bool alreadyRented = db.RentalTracking.Any(r =>
                r.UserID == userId &&
                r.BookID == bookId);

            if (alreadyRented)
            {
                TempData["Message"] = "You have already rented this book and not yet returned it.";
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            var rental = new RentalTracking
            {
                UserID = userId,
                BookID = bookId,
                StartDate = StartDate,
                EndDate = EndDate // custom date
            };

            db.RentalTracking.Add(rental);
            db.SaveChanges();

            TempData["Message"] = $"Book rented from {StartDate.ToShortDateString()} to {EndDate.ToShortDateString()}";
            return RedirectToAction("Details", "Books", new { id = bookId });
            //return RedirectToAction("Index", "RentalHistoryLists");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnBook(int rentalId)
        {
            var rental = db.RentalTracking.FirstOrDefault(r => r.RentalID == rentalId);

            if (rental.EndDate > DateTime.Now)
            {
                rental.EndDate = DateTime.Now;
                db.SaveChanges();

                TempData["Message"] = "Book returned successfully!";
            }
            return RedirectToAction("Index", "RentalHistoryLists");
        }

    }
}