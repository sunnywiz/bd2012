using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BD2012.Code;
using BD2012.Models;

namespace BD2012.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Burndown v2012";
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

    }
}
