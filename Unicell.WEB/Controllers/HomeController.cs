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