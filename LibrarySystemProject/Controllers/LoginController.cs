using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LibrarySystemProject.Models;


namespace LibrarySystemProject.Controllers
{
    public class LoginController : Controller
    {
        private LibraryContext db = new LibraryContext();
    
        [HttpGet]
        public ActionResult Index()
        {
            Login login = new Login();
            return View(login);
        }

        [HttpPost]
        public ActionResult Index(Login login)
        {
            if (ModelState.IsValid)
            {
                // Check if the user exists in the database
                var user = db.Users.FirstOrDefault(u => u.Email == login.Email);

                if (user != null && user.Password == login.Password) // Use hashed passwords in production
                {
                    Session["Firstname"] = user.FirstName;
                    Session["UserID"] = user.UserID;
                    // Create authentication ticket
                    FormsAuthentication.SetAuthCookie(user.FirstName, false);
                    
                    return RedirectToAction("Index", "Home"); // Redirect to home page after login
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(login);
        }

        public ActionResult Logout()
        { 
            Session.Clear(); // Clear all session data
            Session.Abandon(); // Abandon the session completely
            FormsAuthentication.SignOut(); // Sign out the user
            return RedirectToAction("Index", "Home");
        }
    }
}