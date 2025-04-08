using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibrarySystemProject.Models;

namespace LibrarySystemProject.Controllers
{
    public class CategoriesController : Controller
    {
        private LibraryContext db = new LibraryContext();

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file,Category category)
        {
            if (ModelState.IsValid)
            {
                var filename = Path.GetFileName(file.FileName);
                category.Photo = filename;
                db.Categories.Add(category);
                
                var imagesPath = Server.MapPath("~/Images/");
                if (!Directory.Exists(imagesPath))
                {

                    Directory.CreateDirectory(imagesPath); // Create if not exists
                }
                var path = Path.Combine(Server.MapPath("~/Images/"), filename);
                file.SaveAs(path);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file, Category category)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = db.Categories.AsNoTracking().FirstOrDefault(b => b.CategoryID == category.CategoryID);

                if (file != null && file.ContentLength > 0)
                {
                    // Delete the old file if it exists
                    if (!string.IsNullOrEmpty(existingCategory.Photo))
                    {
                        var oldPath = Path.Combine(Server.MapPath("~/Images/"), existingCategory.Photo);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    var imagesPath = Server.MapPath("~/Images/");
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath); // Create if not exists
                    }

                    // Save new file
                    var fileName = Path.GetFileName(file.FileName);
                    var newPath = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(newPath);

                    // Step 4: Update photo path in the book object
                    category.Photo = fileName;
                }
                else
                {
                    // If no new file uploaded, keep the existing photo
                    category.Photo = existingCategory.Photo;
                }
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            var existingCategory = db.Categories.AsNoTracking().FirstOrDefault(c => c.CategoryID == category.CategoryID);

            // Delete the old file if it exists
            if (!string.IsNullOrEmpty(existingCategory.Photo))
            {
                var fullPath = Request.MapPath("~/Images/" + existingCategory.Photo);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
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
