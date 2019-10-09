using Mirante.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
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

        [HttpPost]
        [Route("GetMobile")]
        public JsonResult GetMobile()
        {
            var retorno = Business.WebBLL.getMobile(UsuarioLogado.Usuario.UserID);

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Chat()
        {
            return View();
        }
    }
}