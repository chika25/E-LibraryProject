using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibrarySystemProject.Controllers
{
    public class WishListItemController : Controller
    {
        // GET: WishListItem
        public ActionResult Index()
        {
            return View();
        }
    }
}