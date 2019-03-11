using Dapper;
using Newtonsoft.Json;
using System.Linq;
using Unicell.DTO;

namespace Unicell.DAL
{
    public class DALConnectionMobile
    {
        public string[] SendAcessos(string androidID, string packages)
        {
            return JsonConvert.DeserializeObject<string[]>(packages).Select(package => {
                Utils.DapperConnection.Query("MANTER_ACESSO", new {
                    ANDROID_ID = androidID,
                    PACKAGE_NAME = package.Split('|').First(),
                    ACESSO = package.Split('|').Last()
                });
                return package.Split('|').First();
            }).ToArray();
        }

        public MobileResultDTO SendMobile(string androidID, string geoLocation, string androidStatus)
        {
            return new MobileResultDTO()
            {
                AppAutorizado = Utils.DapperConnection.Query(" MANTER_MOBILE ", new
                {
                    ANDROID_ID = androidID,
                    GEO_LOCALIZACAO = geoLocation,
                    ANDROID_STATUS = androidStatus
                }).Select(item => (string)item.PACKAGE_NAME).ToList(),
                Icon = Utils.DapperConnection.Query(" SELECT CAST(ICON AS CHAR(1)) AS SHOW_ICON FROM MOBILE WHERE ANDROID_ID = @ANDROID_ID ", new {
                    ANDROID_ID = androidID,
                }).Select(item => (string)item.SHOW_ICON == "1").First()
            };
        }
    }
}
