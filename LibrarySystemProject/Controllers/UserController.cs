using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibrarySystemProject.Models;

namespace LibrarySystemProject.Controllers
{
    public class UserController : Controller
    {
        private LibraryContext _context;

        public UserController()
        {
            _context = new LibraryContext(); 
        }

        // GET: User/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User userModel)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.Any(u => u.Email == userModel.Email);
                if (!existingUser)
                {
                    _context.Users.Add(userModel);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Login"); 
                }
                else
                {
                    ModelState.AddModelError("", "Email already exists.");
                }
            }

            return View(userModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}