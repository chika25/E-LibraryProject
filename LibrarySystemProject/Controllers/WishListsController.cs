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
    public class WishListsController : Controller
    {
        private LibraryContext db = new LibraryContext();

        // GET: WishLists
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login"); // Redirect to login if not logged in
            }

            int userId = (int)Session["UserID"];

            var wishlistItems = db.WishListItem
                       .Where(w => w.WishList.UserID == userId)
                       .Include(w => w.Book)  // Include related Book details
                       .ToList();

            return View(wishlistItems);
        }


        // GET: WishLists/Details/5
        // GET: WishLists/Details/5
        public ActionResult Details(int id)
        {
            var wishListItem = db.WishListItem
                                  .Include(w => w.Book)
                                  .FirstOrDefault(w => w.BookID == id);  // Assuming BookID is the foreign key to the Book

            if (wishListItem == null)
            {
                return HttpNotFound();
            }

            return View(wishListItem.Book);  // Pass the Book to the view
        }






        // GET: WishLists/Create
        public ActionResult Create()
        {
            // Get the list of books from the database
            var books = db.Books.Select(b => new { b.BookID, b.Title }).ToList();

            // Pass the list of books to the view
            ViewBag.Books = new SelectList(books, "BookID", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookID")] WishListItem wishListItem)
        {
            if (ModelState.IsValid)
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Login"); // Redirect if not logged in
                }

                int userId = (int)Session["UserID"];

                // Find an existing wishlist for user 1
                var userWishList = db.WishList.FirstOrDefault(w => w.UserID == userId);

                // If no wishlist exists, create a new one
                if (userWishList == null)
                {
                    userWishList = new WishList { UserID = userId };
                    db.WishList.Add(userWishList);
                    db.SaveChanges(); // Save first to get the generated WishListID
                }

                // Assign the found or created WishListID to the new WishListItem
                wishListItem.WishListID = userWishList.WishListID;

                // Add the WishListItem entry
                db.WishListItem.Add(wishListItem);
                db.SaveChanges();

                return RedirectToAction("Index", new { userId = userId });
            }

            // Reload books list for dropdown in case of validation errors
            var books = db.Books.Select(b => new { b.BookID, b.Title }).ToList();
            ViewBag.Books = new SelectList(books, "BookID", "Title");

            return View(wishListItem);
        }





        // GET: WishLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the WishListItem by its ID
            WishListItem wishListItem = db.WishListItem
                                           .Include(w => w.Book)  // Include the Book details
                                           .FirstOrDefault(w => w.WishListItemID == id);

            if (wishListItem == null)
            {
                return HttpNotFound();
            }

            // Pass the WishListItem to the view
            return View(wishListItem);
        }

        // POST: WishLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Find the WishListItem by its ID
            WishListItem wishListItem = db.WishListItem.Find(id);

            if (wishListItem == null)
            {
                return HttpNotFound();
            }

            // Remove the WishListItem from the database
            db.WishListItem.Remove(wishListItem);
            db.SaveChanges();

            // Redirect back to the wishlist index page
            return RedirectToAction("Index");
        }


    }
}
