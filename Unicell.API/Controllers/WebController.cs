using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Unicell.API.Login;
using Unicell.API.Models;
using Unicell.DAL;
using Unicell.DTO;

namespace Unicell.API.Controllers
{
    [RoutePrefix("Web")]
    public class WebController : ApiController
    {
        public MobileDTO getMobile([FromBody]int UserID)
        {
            return DALConnectionWEB.getMobile(UserID);
        }

        [HttpPost]
        [Route("SignIn")]
        public UsuarioDTO SignIn([FromBody]string Email, [FromBody]string Password)
        {
            return DALConnectionWEB.SignIn(Email, Password);
        }

        public bool SendMobile([FromBody]string androidID, [FromBody] string geoLocation, [FromBody] string androidStatus, [FromBody] string nomeFuncionario, [FromBody] string UserID, [FromBody] bool Icon)
        {
            return DALConnectionWEB.SendMobile(androidID, geoLocation, androidStatus, nomeFuncionario, UserID, Icon);
        }

        public bool SendACessoMobile([FromBody]string androidID, [FromBody] Nullable<int> id_app, [FromBody] string packageName, [FromBody] string descricao, [FromBody] string dataCoverSmall, [FromBody] string dataCoverLarge, [FromBody] char incluir)
        {
            return DALConnectionWEB.SendACessoMobile(androidID, id_app, packageName, descricao, dataCoverSmall, dataCoverLarge, incluir);
        }

        public void SetIcon([FromBody]string icon, [FromBody] string androidId)
        {
            DALConnectionWEB.SetIcon(icon, androidId);
        }
        
        public List<AppMetadataDTO> getAcessos([FromBody] string androidId)
        {
            return DALConnectionWEB.getAcessos(androidId);
        }

        public List<AppMetadataDTO> getApps([FromBody] List<string> packageNames, [FromBody] string androidId)
        {
            return DALConnectionWEB.getApps(packageNames, androidId);
        }

        public GoogleAdressDTO getAdress([FromBody]string geo)
        {
            try
            {
                GoogleAdressDTO retorno = JsonConvert.DeserializeObject<GoogleAdressDTO>(Utils.getResponseFromServerAsync(
               string.Format("https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyBtyISrxBBOMjbK3JwpaDFXA4Spazqc5Ck&latlng={0}&sensor=false", geo.Replace(":", ",").Replace("\"", ""))));

                while (retorno.status != "OK")
                {
                    retorno = JsonConvert.DeserializeObject<GoogleAdressDTO>(Utils.getResponseFromServerAsync(
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
        /// Carrega lista de aplicativos da PlayStore
        /// </summary>
        /// <param name="name">Nome do aplicativo</param>
        /// <returns>Lista de aplicativos contendo Nome, PackageName e imagens de preview.</returns>
        [HttpPost]
        [Route("getAppList")]
        public List<AppMetadataDTO> getAppList([FromBody]getAppModel model)
        {
            List<AppMetadataDTO> retorno = Utils.getAppListByRegex(model.name,
                "<div class=\"[^\\\"]*\" title=\"[^\\\"]*\"|<a href=\"/store/apps/details?.[^\\\"]*\" aria-hidden=\"[^\\\"]*\" tabindex=\"[^\\\"]*\" class=\"[^\\\"]*\"></a>|<img.+?\"https://lh3.googleusercontent.com.+?>",
                "https://play.google.com/store/search?c=apps&q=");

            //List<AppMetadataDTO> retornoDB = getApps(retorno.Select(x => x.PackageName).ToList(), model.androidId);
            return retorno;
        }

        [HttpGet]
        [Route("SendSMS")]
        public void SendSMS()
        {
            List<string> telefones = DALConnectionWEB.SendSMS();

            var accountSid = "AC451369c174f3d440cd24d39a31d87754";
            var authToken = "ac33558425a4d4f15a6e383f34e58cb2";

            TwilioClient.Init(accountSid, authToken);

            foreach (var item in telefones)
            {
                var messageOptions = new CreateMessageOptions(new PhoneNumber(item));
                messageOptions.From = new PhoneNumber("+12139842214");
                messageOptions.Body = "https://install.appcenter.ms/users/grupounicell/apps/unicell-locker/distribution_groups/distribute";

                var message = MessageResource.Create(messageOptions);
            }
        }
    }
}
