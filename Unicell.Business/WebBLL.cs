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
        public static List<AppMetadataDTO> getAppListByRegex(string name, string pattern, string url, bool primeiro = false, string packageName = "")
        {
            return DAL.Utils.getAppListByRegex(name, pattern, url, primeiro, packageName);
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
        public static List<AppMetadata> getAppList(string name, string androidId)
        {
            return DAL.DALConnectionWEB.getAppList(name, androidId);
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

        public static FuncionarioDTO GetFuncionario(int? ID_EMPRESA, int? QtdPorPagina, int? Pagina, string search)
        { return DAL.DALConnectionWEB.GetFuncionario(ID_EMPRESA, QtdPorPagina, Pagina, search); }

        public static CargoDTO GetCargo(int? ID_EMPRESA, int? QtdPorPagina, int? Pagina, string search)
        { return DAL.DALConnectionWEB.GetCargo(ID_EMPRESA, QtdPorPagina, Pagina, search); }

        public static SucessoDTO SetCargo(int? ID_EMPRESA, string NM_CARGO, int? ID)
        { return DAL.DALConnectionWEB.SetCargo(ID_EMPRESA, NM_CARGO, ID); }

        public static SucessoDTO SetFuncionario(int? ID_EMPRESA, string NM_FUNCIONARIO, int? ID, int? ID_CARGO, string CPF, string RG, bool? GENERO)
        { return DAL.DALConnectionWEB.SetFuncionario(ID_EMPRESA, NM_FUNCIONARIO, ID, ID_CARGO, CPF, RG, GENERO); }
    }
}
