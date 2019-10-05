using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unicell.WEB.Utils;

namespace Unicell.WEB.Controllers
{
    [RoutePrefix("Home")]
    [Autorizacao]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GoogleMaps()
        {
            return View();
        }
    }
}