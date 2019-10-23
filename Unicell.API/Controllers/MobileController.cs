using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Unicell.API.Models;
using Unicell.DAL;
using Unicell.DTO;

namespace Unicell.API.Controllers
{
    [RoutePrefix("Mobile")]
    public class MobileController : ApiController
    {
        [HttpPost]
        [Route("SendAcessos")]
        public string[] SendAcessos([FromBody] SendAcessosModel model)
        {
            return DALConnectionMobile.SendAcessos(model.androidID, model.packages);
        }

        [HttpPost]
        [Route("SendMobile")]
        public MobileResultDTO SendMobile([FromBody] SendMobileModel model)
        {
            return DALConnectionMobile.SendMobile(model.androidID, model.geoLocation, model.androidStatus, model.telefones, model.isCharging, model.signalStrength, model.chargeLevel);
        }

        [HttpPost]
        [Route("SendEmpresa")]
        public void SendEmpresa([FromBody] ModelEmpresaMobile model)
        {
            DALConnectionMobile.SendEmpresa(model.ID_EMPRESA, model.ANDROID_ID);
        }
    }
}
