using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unicell.DTO;

namespace Unicell.DAL
{
    public static class Utils
    {
        /// <summary>
        /// Retorna aplicativos da página PlayStore baseado em regex.
        /// </summary>
        /// <param name="name">Nome do aplicativo</param>
        /// <param name="pattern">String de filtro para retornar itens DOM que contém as informações há serem retornadas.</param>
        /// <param name="url">Link dá pagina a ser mapeada os aplicativos.</param>
        /// <returns>Lista de aplicativos contendo Nome, PackageName e imagens de preview.</returns>
        public static List<AppMetadataDTO> getAppListByRegex(string name, string pattern, string url, bool primeiro = false, string packageName = "")
        {
            MatchCollection b = new Regex(pattern).Matches(getResponseFromServerAsync(url + name));
            int pipe = pattern.Count(x => x.Equals('|'));

            var retorno = (b.Count > 0) ? b.Cast<Match>().Take(Math.Max(0, b.Count - pipe))
                            .Select((m, i) => new {
                                index = i,
                                value = m.Value + Enumerable.Range(i, pipe)
                                                .Select(x => b[x + 1].Value)
                                                .Aggregate((a, c) => a + c)
                            })
                            .Where(m => m.index % (pipe + 1) == 0)
                            .Select(m => new AppMetadataDTO()
                            {
                                Descricao = GetVal(m.value, "alt"),
                                dataCoverLarge = GetVal(m.value, "data-cover-large"),
                                dataCoverSmall = GetVal(m.value, "data-cover-small"),
                                PackageName = (packageName != "") ? packageName : GetVal(m.value, "data-docid")
                            }).ToList() : new List<AppMetadataDTO>();

            return (primeiro) ? new List<AppMetadataDTO>() { retorno.First() } : retorno;
        }

        /// <summary>
        /// Recupera valor do elemento DOM
        /// </summary>
        /// <param name="valor">HTML do elemento DOM.</param>
        /// <param name="coluna">Nome da propriedade do elemento DOM há ser recuperada.</param>
        /// <returns>Valor filtrado do elemento DOM</returns>
        public static string GetVal(string valor, string coluna)
        {
            try
            {
                return Regex.Match(
                         Regex.Matches(valor, "(\\S+)=[\"']?((?:.(?![\"']?\\s+(?:\\S+)=|[>\"']))+.)[\"']?")
                        .Cast<Match>().Where(x => x.Value.Contains(coluna)).First().Value, "\"([^\"]*)\""
                    ).Value.Replace("\"", "");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Recupera informações de forma bruta da página de aplicativos PlayStore
        /// </summary>
        /// <param name="link">Link dá pagina a ser mapeada os aplicativos.</param>
        /// <returns>HTML completo</returns>
        public static string getResponseFromServerAsync(string link)
        {

            WebRequest req = WebRequest.Create(link);
            req.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            WebResponse responseTask = req.GetResponse();
            using (WebResponse response = responseTask)
            {
                HttpWebResponse resp = (HttpWebResponse)response;
                Stream respStream = resp.GetResponseStream();
                StreamReader sr = new StreamReader(respStream);
                string responseFromServer = sr.ReadToEnd();

                sr.Close();
                respStream.Close();
                resp.Close();
                return responseFromServer;
            }

        }

        public static SqlConnection DapperConnection
        {
            get
            {
                return new SqlConnection(ConnectionString);
            }
        }

        public static string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["UNICELLLOCKER"].ConnectionString;
            }
        }

    }
}
