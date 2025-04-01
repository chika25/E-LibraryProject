﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibrarySystemProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
        

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }
        public ActionResult Donate()
        {
            return View();
        }
    }
}