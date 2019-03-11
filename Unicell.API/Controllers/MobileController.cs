using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Unicell.DAL;
using Unicell.DTO;

namespace Unicell.API.Controllers
{
    public class MobileController : ApiController
    {
        public string[] SendAcessos([FromBody]string androidID, [FromBody]string packages)
        {
            return new DALConnectionMobile().SendAcessos(androidID, packages);
        }

        public MobileResultDTO SendMobile([FromBody]string androidID, [FromBody]string geoLocation, [FromBody]string androidStatus)
        {
            return new DALConnectionMobile().SendMobile(androidID, geoLocation, androidStatus);
        }
    }
}
