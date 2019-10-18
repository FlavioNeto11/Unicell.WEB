using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicell.DTO;

namespace Unicell.Business
{
    public class WebBLL
    {
        public static MobileDTO getMobile(int UserID, int? QtdPorPagina = 10000, int? Pagina = 1, string search = null)
        {
            return DAL.DALConnectionWEB.getMobile(UserID, QtdPorPagina, Pagina, search);
        }

        public static UsuarioDTO SignIn(string email, string password)
        {
            return DAL.DALConnectionWEB.SignIn(email, password);
        }

        public static bool SendMobile(string androidID, string geoLocation, string androidStatus, string nomeFuncionario, string UserID, bool Icon)
        {
            return DAL.DALConnectionWEB.SendMobile(androidID, geoLocation, androidStatus, nomeFuncionario, UserID, Icon);
        }

        public static bool SendACessoMobile(string androidID, Nullable<int> id_app, string packageName, string descricao, string dataCoverSmall, string dataCoverLarge, char incluir)
        {
            return DAL.DALConnectionWEB.SendACessoMobile(androidID, id_app, packageName, descricao, dataCoverSmall, dataCoverLarge, incluir);
        }

        public static void SetIcon(string icon, string androidId)
        {
            DAL.DALConnectionWEB.SetIcon(icon, androidId);
        }

        public static List<AppMetadataDTO> getAcessos(string androidId)
        {
            return DAL.DALConnectionWEB.getAcessos(androidId);
        }

        public static List<string> SendSMS()
        {
            return DAL.DALConnectionWEB.SendSMS();
        }

        public static List<AppMetadataDTO> getApps(List<string> packageNames, string androidId)
        {
            return DAL.DALConnectionWEB.getApps(packageNames, androidId);
        }
    }
}
