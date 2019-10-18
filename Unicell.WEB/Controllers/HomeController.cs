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
            ViewBag.UserName = UsuarioLogado.Usuario.UserName;
            return View();
        }

        public ActionResult ConfigurarDispositivos()
        {
            return View();
        }

        [HttpPost]
        [HandleErrorWithAjaxFilter]
        [Route("GetMobile")]
        public JsonResult GetMobile(int? draw, int? start, int? length, string search)
        {
            if (draw != null)
            {
                int? page = (start.HasValue ? start.Value / length : 0) + 1;
                var retorno = Business.WebBLL.getMobile(UsuarioLogado.Usuario.UserID, length ?? 5, page.Value, search);
                retorno.draw = draw;
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var retorno = Business.WebBLL.getMobile(UsuarioLogado.Usuario.UserID);
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Chat()
        {
            return View();
        }
    }
}