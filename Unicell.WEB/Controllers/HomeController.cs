using Mirante.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Unicell.WEB.Utils;
using Unicell.DTO;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using RestSharp;

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

        public ActionResult ConfigurarCargo()
        {
            return View();
        }

        public ActionResult ConfigurarFuncionarios()
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
                try
                {
                    foreach (var item in retorno.data)
                    {
                        if (item.GEO_LOCALIZACAO != string.Empty && item.GEO_LOCALIZACAO != null)
                            item.ENDERECO = getAdress(item.GEO_LOCALIZACAO).results[0].formatted_address;
                    }
                }
                catch (Exception ex)
                {
                    var e = ex;
                }
               
                retorno.draw = draw;
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var retorno = Business.WebBLL.getMobile(UsuarioLogado.Usuario.UserID, 10000,1,search);
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [HandleErrorWithAjaxFilter]
        [Route("GetCargo")]
        public JsonResult GetCargo(int? draw, int? start, int? length, string search)
        {
            if (draw != null)
            {
                int? page = (start.HasValue ? start.Value / length : 0) + 1;
                var retorno = Business.WebBLL.GetCargo(UsuarioLogado.Usuario.ID_Empresa, length ?? 5, page.Value, search);
                retorno.draw = draw;
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var retorno = Business.WebBLL.GetCargo(UsuarioLogado.Usuario.ID_Empresa, 10000, 1, search);
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [HandleErrorWithAjaxFilter]
        [Route("SetCargo")]
        public JsonResult SetCargo(string NM_CARGO, int? ID)
        {
            var retorno = Business.WebBLL.SetCargo(UsuarioLogado.Usuario.ID_Empresa, NM_CARGO, ID);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [HandleErrorWithAjaxFilter]
        [Route("SetFuncionario")]
        public JsonResult SetFuncionario(string NM_FUNCIONARIO, int? ID, int? ID_CARGO, string CPF, string RG, bool? GENERO)
        {
            var retorno = Business.WebBLL.SetFuncionario(UsuarioLogado.Usuario.ID_Empresa, NM_FUNCIONARIO, ID, ID_CARGO, CPF, RG, GENERO);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [HandleErrorWithAjaxFilter]
        [Route("getApps")]
        public JsonResult getApps(string nomeAplicativo, string androidID)
        {
            if (nomeAplicativo != null && nomeAplicativo.Trim() != string.Empty)
            {
                List<AppMetadataDTO> retorno = Business.WebBLL.getAppListByRegex(nomeAplicativo,
                "<div class=\"[^\\\"]*\" title=\"[^\\\"]*\"|<a href=\"/store/apps/details?.[^\\\"]*\" aria-hidden=\"[^\\\"]*\" tabindex=\"[^\\\"]*\" class=\"[^\\\"]*\"></a>|<img.+?\"https://lh3.googleusercontent.com.+?>",
                "https://play.google.com/store/search?c=apps&q=");

                List<AppMetadataDTO> retornoDB = Business.WebBLL.getApps(retorno.Select(x => x.PackageName).ToList(), androidID);
               
                return Json(JsonConvert.SerializeObject(retornoDB.Union(retorno, new AppMetadataComparer()).ToList()));
            }

            return Json(JsonConvert.SerializeObject(new List<AppMetadata>()));
        }

        [HttpPost]
        [HandleErrorWithAjaxFilter]
        [Route("getAppsList")]
        public JsonResult getAppsList(string androidID)
        {
            return Json(JsonConvert.SerializeObject(Business.WebBLL.getApps(new string[] { }.ToList(), androidID)));
        }


        [HttpPost]
        [HandleErrorWithAjaxFilter]
        [Route("getAcessos")]
        public JsonResult getAcessos(string androidID)
        {
            var retorno = Business.WebBLL.getAcessos(androidID);
            return Json(JsonConvert.SerializeObject(retorno));
        }


        [HttpPost]
        [HandleErrorWithAjaxFilter]
        [Route("sendApps")]
        public JsonResult sendApps(string androidID, Nullable<int> id_app, string packageName, string descricao, string dataCoverSmall, string dataCoverLarge, char incluir)
        {
            return Json(JsonConvert.SerializeObject(Business.WebBLL.SendACessoMobile(androidID, (id_app == 0) ? null : id_app, packageName, descricao, dataCoverSmall, dataCoverLarge, incluir)));
        }

        [HttpPost]
        [HandleErrorWithAjaxFilter]
        [Route("GetFuncionario")]
        public JsonResult GetFuncionario(int? draw, int? start, int? length, string search)
        {
            if (draw != null)
            {
                int? page = (start.HasValue ? start.Value / length : 0) + 1;
                var retorno = Business.WebBLL.GetFuncionario(UsuarioLogado.Usuario.ID_Empresa, length ?? 5, page.Value, search);
                retorno.draw = draw;
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var retorno = Business.WebBLL.GetFuncionario(UsuarioLogado.Usuario.ID_Empresa, 10000, 1, search);
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
        }

        public GoogleAdressDTO getAdress(string geo)
        {
            try
            {
                GoogleAdressDTO retorno = JsonConvert.DeserializeObject<GoogleAdressDTO>(getResponseFromServerAsync(
               string.Format("https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyBtyISrxBBOMjbK3JwpaDFXA4Spazqc5Ck&latlng={0}&sensor=false", geo.Replace(":", ",").Replace("\"", ""))));

                while (retorno.status != "OK")
                {
                    retorno = JsonConvert.DeserializeObject<GoogleAdressDTO>(getResponseFromServerAsync(
                   string.Format("https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyBtyISrxBBOMjbK3JwpaDFXA4Spazqc5Ck&latlng={0}&sensor=false", geo.Replace(":", ",").Replace("\"", ""))));
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return new GoogleAdressDTO();
            }
        }

        /// <summary>
        /// Recupera informações de forma bruta da página de aplicativos PlayStore
        /// </summary>
        /// <param name="link">Link dá pagina a ser mapeada os aplicativos.</param>
        /// <returns>HTML completo</returns>
        private string getResponseFromServerAsync(string link)
        {
            var client = new RestClient(link);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "maps.googleapis.com");
            request.AddHeader("Postman-Token", "af6bb837-aba1-465c-8d87-5d01a774327f,4c384b87-936b-4b15-8e7c-ce2eec1936c5");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.17.1");
            IRestResponse response = client.Execute(request);

            return response.Content;
        }

        public ActionResult Chat()
        {
            return View();
        }
    }
}