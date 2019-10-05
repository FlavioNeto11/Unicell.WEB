using Mirante.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Unicell.WEB.Utils;

namespace Unicell.WEB.Controllers
{
    [RoutePrefix("Login")]
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("LoginIn")]
        public JsonResult LoginIn(string email, string password)
        {
            var retorno = Business.WebBLL.SignIn(email, password);

            if (retorno != null)
            {
                UsuarioLogado.Usuario = new DTO.UsuarioDTO() {
                    UserID = retorno.UserID,
                    UserName = retorno.UserName
                };
            }

            return Json(JsonConvert.SerializeObject(retorno));
        }

        public ActionResult Logout()
        {
            LimparSessao();
            return RedirectToAction("Index", "Login");
        }

        private void LimparSessao()
        {
            Utility.Cookies.Clean(Utility.Cookies.USUARIO);

            FormsAuthentication.SignOut();
            Session.Abandon();

            Session.Clear();
            Session.RemoveAll();
        }
    }
}