using System;
using System.Collections.Generic;
using System.Linq;
using Unicell.DTO;
using Dapper;

namespace Unicell.DAL
{
    public class DALConnectionWEB
    {
        public static List<MobileDTO> getMobile(string UserID)
        {
            return Utils.DapperConnection.Query(" SELECT M.ANDROID_ID, M.GEO_LOCALIZACAO, M.ULTIMO_ACESSO, M.ANDROID_STATUS, F.NM_FUNCIONARIO, M.ICON FROM MOBILE M "
                                                         + " LEFT JOIN FUNCIONARIO F ON M.ID_FUNCIONARIO = F.ID "
                                                         + " LEFT JOIN USERPROFILE U ON U.ID_EMPRESA = F.ID_EMPRESA "
                                                         + " WHERE U.USERNAME = @USERID OR F.ID IS NULL ", new { USERID = UserID }).Select(
                item => new MobileDTO() {
                    AndroidID = item.ANDROID_ID,
                    GeoLocalizacao = item.GEO_LOCALIZACAO,
                    UltimoAcesso = item.ULTIMO_ACESSO.ToString(),
                    AndroidStatus = item.ANDROID_STATUS,
                    NomeFuncionario = item.NM_FUNCIONARIO,
                    Icon = item.ICON
                }).ToList();

        }

        public static bool SendMobile(string androidID, string geoLocation, string androidStatus, string nomeFuncionario, string UserID, bool Icon)
        {
            try
            {
                Utils.DapperConnection.Execute("MANTER_MOBILE", new
                {
                    ANDROID_ID = androidID,
                    GEO_LOCALIZACAO = geoLocation,
                    ANDROID_STATUS = androidStatus,
                    NOME_FUNCIONARIO = nomeFuncionario,
                    USERID = UserID,
                    ICON = Icon
                });
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool SendACessoMobile(string androidID, Nullable<int> id_app, string packageName, string descricao, string dataCoverSmall, string dataCoverLarge, char incluir)
        {
            try
            {
                Utils.DapperConnection.Execute("MANTER_ACESSO_MOBILE", new
                {
                    ANDROID_ID = androidID,
                    ID_APP = id_app,
                    PACKAGE_NAME = packageName,
                    DESCRICAO = descricao,
                    DATA_COVER_SMALL = dataCoverSmall,
                    DATA_COVER_LARGE = dataCoverLarge,
                    INCLUIR = incluir
                });
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static void SetIcon(string icon, string androidId)
        {
            Utils.DapperConnection.Execute("UPDATE MOBILE SET ICON = @ICON WHERE ANDROID_ID = @ANDROID_ID", new
            {
                ANDROID_ID = androidId,
                ICON = icon == "1"
            });
        }

        public static List<AppMetadataDTO> getAcessos(string androidId)
        {
            var query = Utils.DapperConnection.Query("SELECT AC.PACKAGE_NAME, AC.ACESSO, A.DATA_COVER_SMALL, A.DATA_COVER_LARGE, A.DESCRICAO FROM APP_ACESSO AC "
                                                     + "   LEFT JOIN  APP A ON A.PACKAGE_NAME = AC.PACKAGE_NAME"
                                                     + "   WHERE AC.ANDROID_ID = @ANDROID_ID", new { ANDROID_ID = androidId });

            var packages = query.Where(item => item.DATA_COVER_SMALL == null || item.DATA_COVER_LARGE == null).Select(item => item.PACKAGE_NAME).Distinct();

            var itens = packages.SelectMany(item => (List<AppMetadataDTO>)(Utils.getAppListByRegex(item.ToString(),
                                   "<img.+?src=[\"'](.+?)[\"'].+?>|<span class=\"preview-overlay-container\" data-docid=[\"'](.+?)[\"'].+?>",
                                   "https://play.google.com/store/search?c=apps&q=", true, item.ToString())));
            
            return query.Select(meta => { return (meta.DATA_COVER_SMALL == null || meta.DATA_COVER_LARGE == null) ?
                    itens.Where(item => item.PackageName == meta.PACKAGE_NAME.ToString()).Select(item => { item.Acesso = meta.ACESSO.ToString(); return item; }).First() :
                 new AppMetadataDTO()  { Acesso = meta.ACESSO.ToString(), dataCoverLarge = meta.DATA_COVER_LARGE, dataCoverSmall = meta.DATA_COVER_SMALL, Descricao = meta.DESCRICAO, PackageName = meta.PACKAGE_NAME
                };
            }).ToList();
         
        }

        public static List<string> SendSMS()
        {
            return Utils.DapperConnection.Query(" SELECT TELEFONE FROM NUMERO_TELEFONE ").Select(item => (string)item.TELEFONE).ToList();
        }

        public static List<AppMetadataDTO> getApps(List<string> packageNames, string androidId)
        {
            return Utils.DapperConnection.Query(" SELECT A.ID, A.PACKAGE_NAME, A.DESCRICAO, A.DATA_COVER_SMALL, A.DATA_COVER_LARGE, " +
                                                            " CASE WHEN AP.ID_APP IS NULL THEN('N') ELSE 'S' END AS AUTORIZADO " +
                                                            " FROM APP A " +
                                                            " LEFT JOIN APP_AUTORIZADO AP ON A.ID = AP.ID_APP AND AP.ID_MOBILE = @ANDROID_ID " +
                                                            " WHERE AP.ID_APP IS NOT NULL OR(AP.ID_APP IS NULL AND A.PACKAGE_NAME IN(@PACKAGE_NAME))", 
                                                            new {
                                                                PACKAGE_NAME = string.Join(",", packageNames),
                                                                ANDROID_ID = androidId
                                                            }).Select(
            item => new AppMetadataDTO()
            {
                Id = item.ID,
                PackageName =item.PACKAGE_NAME,
                Descricao = item.DESCRICAO,
                dataCoverSmall = item.DATA_COVER_SMALL,
                dataCoverLarge = item.DATA_COVER_LARGE,
                Autorizado = item.AUTORIZADO
            }).ToList();
        }
    }
}
