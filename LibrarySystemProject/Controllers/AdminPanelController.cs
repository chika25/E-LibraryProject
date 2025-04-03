using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LibrarySystemProject.Models;

namespace LibrarySystemProject.Controllers
{
    public class AdminPanelController : Controller
    {

        private LibraryContext db = new LibraryContext();
        // GET: AdminPanel

        public ActionResult Index()
        {
            List<User> users;
            users = db.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public ActionResult CheckUserAdminRole(int userId)
        {
            var user = db.Users.FirstOrDefault(u => u.UserID == userId);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            // Join UserRoles with Roles to check if the user has the "Admin" role (RoleId = 1)
            var hasAdminRole = db.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(db.Roles, ur => ur.RoleId, r => r.RoleId, (ur, r) => r.Name)
                .Any(roleName => roleName == "Admin");

            return Json(new { success = true, isAdmin = hasAdminRole });
        }


        [HttpPost]
        public JsonResult ToggleAdminRole(int userId)
        {
            var user = db.Users.FirstOrDefault(u => u.UserID == userId);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            // Get the user's current roles
            var userRole = db.UserRoles.SingleOrDefault(ur => ur.UserId == user.UserID);
            var adminRole = db.Roles.FirstOrDefault(r => r.Name == "Admin");

            if (userRole == null)
            {
                // Add the Admin role
                db.UserRoles.Add(new UserRole { UserId = user.UserID, RoleId = adminRole.RoleId });
                db.SaveChanges();
                return Json(new { success = true, isAdmin = true });
            }
            else
            {
                // Remove the Admin role
                db.UserRoles.Remove(userRole);
                db.SaveChanges();
                return Json(new { success = true, isAdmin = false });
            }
            
        }

        // GET: AdminPanel/Edit/5
        public ActionResult Edit(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);  // This view should display a form with the user's current data.
        }

        // POST: AdminPanel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                // Optionally, you can load the existing user from the database and update specific fields.
                db.Entry(updatedUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(updatedUser);
        }

        // GET: AdminPanel/Delete/5
        public ActionResult Delete(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);  // This view should ask for confirmation before deletion.
        }

        // POST: AdminPanel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }

}