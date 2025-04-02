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
    public class BooksController : Controller
    {
        private LibraryContext db = new LibraryContext();

        // GET: Books
        public ActionResult Index(int? categoryId)
        {
            List<Book> books;

            if (categoryId.HasValue)
            {
                books = db.Books.Where(book => book.CategoryID == categoryId.Value).ToList();
            }
            else
            {
                books = db.Books.ToList();
            }

            ViewBag.CategoryId = categoryId; // Pass categoryId to the view

            //pass wishlistsid
            if (Session["UserID"] != null)
            {
                int userId = (int)Session["UserID"];
                var userWishList = db.WishList.FirstOrDefault(w => w.UserID == userId);

                List<int> wishListBookIds = new List<int>(); // List of BookIDs in wishlist

                if (userWishList != null)
                {
                    wishListBookIds = db.WishListItem
                        .Where(w => w.WishListID == userWishList.WishListID)
                        .Select(w => w.BookID)
                        .ToList();
                }

                ViewBag.WishListBookIds = wishListBookIds;
            }




            return View(books);
        }

        public ActionResult SearchBooks(string keyword)
        {
            // Search for categories that start with the keyword
            var categories = db.Categories
                .Where(c => c.CategoryName.StartsWith(keyword))
                .ToList();

            // Search for books whose titles or authors start with the keyword
            var books = db.Books
                .Where(b => b.Title.StartsWith(keyword) || b.Author.StartsWith(keyword))
                .ToList();

            // Create a result object to hold the filtered data
            var result = new BookSearchViewModel
            {
                Categories = categories,
                Books = books
            };

            // Return the partial view that displays the search results
            return PartialView("_SearchResultList", result);
        }

        public JsonResult AutocompleteSearch(string keyword)
        {
            var bookTitles = db.Books
                .Where(b => b.Title.StartsWith(keyword))
                .Select(b => new { Label = b.Title, ID = b.BookID, Type = "Title" }) 
                .ToList();

            var authors = db.Books
                .Where(b => b.Author.StartsWith(keyword))
                .Select(b => new { Label = b.Author, ID = b.BookID, Type = "Author" }) 
                .Distinct()
                .Take(10)
                .ToList();

            var categories = db.Categories
                .Where(c => c.CategoryName.StartsWith(keyword))
                .Select(c => new { Label = c.CategoryName, ID = c.CategoryID, Type = "Category" }) 
                .Take(10)
                .ToList();

            var combinedResults = bookTitles
                .Concat(authors)
                .Concat(categories)
                .ToList();

            return Json(combinedResults, JsonRequestBehavior.AllowGet);
        }



        // GET: Books/Details/5
        public ActionResult Details(int? id, int? categoryId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Book book = db.Books.Find(id);

            if (book == null)
            {
                return HttpNotFound();
            }
            // Store the category ID for use in the view
            ViewBag.CategoryId = categoryId; 
            return View(book);
        }


        // GET: Books/Create
        public ActionResult Create(int? categoryId)
        {
            if (categoryId.HasValue)
            {
                ViewBag.Category = categoryId;
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", categoryId);
            return View();
        }


        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookID,Title,Author,ISBN,PublicationYear,CategoryID")] Book book, int? categoryId)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                if (categoryId.HasValue)
                {
                    // If categoryId is provided, redirect to the category-specific list, otherwise redirect to the general list
                    return RedirectToAction("Index", new { categoryId = categoryId ?? (object)null });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            if (categoryId.HasValue)
            {
                ViewBag.Category = categoryId;
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
            return View(book);
        }


        // GET: Books/Edit/5
        public ActionResult Edit(int? id, int? categoryId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            if (categoryId.HasValue)
            {
                ViewBag.Category = categoryId;
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
            return View(book);
        }


        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,Title,Author,ISBN,PublicationYear,CategoryID")] Book book, int? categoryId)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();

                if (categoryId.HasValue)
                {
                    // If categoryId is provided, redirect to the category-specific list
                    return RedirectToAction("Index", new { categoryId = categoryId });
                }
                else
                {
                    // If no categoryId, redirect to the general list
                    return RedirectToAction("Index");
                }
            }

            if (categoryId.HasValue)
            {
                ViewBag.Category = categoryId;
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
            return View(book);
        }




        // GET: Books/Delete/5
        public ActionResult Delete(int? id, int? categoryId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            if (categoryId.HasValue)
            {
                ViewBag.CategoryId = categoryId; // Store categoryId to retain it after deletion
            }
            
            
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? categoryId)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();

            if (categoryId.HasValue)
            {
                return RedirectToAction("Index", new { categoryId = categoryId.Value });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
