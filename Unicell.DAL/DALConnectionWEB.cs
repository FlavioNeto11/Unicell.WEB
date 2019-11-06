using System;
using System.Collections.Generic;
using System.Linq;
using Unicell.DTO;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Unicell.DAL
{
    public static class DALConnectionWEB
    {
        public static MobileDTO getMobile(int ID_EMPRESA, int? QtdPorPagina = 10000, int? Pagina = 1, string search = null)
        {
            try
            {
                var query = Utils.DapperConnection.QueryMultiple("GET_MOBILE", new
                {
                    @ID_EMPRESA = ID_EMPRESA,
                    QtdPorPagina = QtdPorPagina,
                    Pagina = Pagina,
                    search = search,
                }, commandType: CommandType.StoredProcedure);


                return new MobileDTO()
                {
                    data = query.Read<MobileDTO.Result>().ToList(),
                    sucesso = query.ReadSingle<SucessoDTO>()
                };
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }

        }

        public static UsuarioDTO SignIn(string email, string password)
        {
            try
            {
                return Utils.DapperConnection.Query<UsuarioDTO>("SIGN_IN", new
                {
                    EMAIL = email,
                    PASSWORD = password
                }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static bool SendMobile(string androidID, string geoLocation, string androidStatus, string nomeFuncionario, string UserID, bool Icon)
        {
            try
            {
                Utils.DapperConnection.Execute("MANTER_MOBILE", new
                {
                    @ANDROID_ID = androidID,
                    @GEO_LOCALIZACAO = geoLocation,
                    @ANDROID_STATUS = androidStatus,
                    @NOME_FUNCIONARIO = nomeFuncionario,
                    @USERID = UserID,
                    @ICON = Icon
                }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool SendACessoMobile(string androidID, Nullable<int> id_app, string packageName, string descricao, string dataCoverSmall, string dataCoverLarge, bool incluir, bool autorizar)
        {
            try
            {
                Utils.DapperConnection.Execute("MANTER_ACESSO_MOBILE", new
                {
                    @ANDROID_ID = androidID,
                    @ID_APP = id_app,
                    @PACKAGE_NAME = packageName,
                    @DESCRICAO = descricao,
                    @DATA_COVER_SMALL = dataCoverSmall,
                    @DATA_COVER_LARGE = dataCoverLarge,
                    @INCLUIR = incluir,
                    @AUTORIZAR = autorizar
                }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
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

        public static AcessoDTO GetAcessos(string ANDROID_ID, int? QtdPorPagina, int? Pagina, string search)
        {
            var query = Utils.DapperConnection.QueryMultiple("GET_ACESSOS", new
            {
                ANDROID_ID = ANDROID_ID,
                QtdPorPagina = QtdPorPagina,
                Pagina = Pagina,
                search = search,
            }, commandType: CommandType.StoredProcedure);
            /*
            var data = query.Read<AcessoDTO.Result>().ToList();

            var packages = data.Where(item => item.DATA_COVER_SMALL == null || item.DATA_COVER_LARGE == null).Select(item => item.PACKAGE_NAME).Distinct();

            var itens = packages.SelectMany(item => (List<AcessoDTO.Result>)(Utils.getAppListByRegex(item.ToString(),
                                   "<img.+?src=[\"'](.+?)[\"'].+?>|<span class=\"preview-overlay-container\" data-docid=[\"'](.+?)[\"'].+?>",
                                   "https://play.google.com/store/search?c=apps&q=", true, item.ToString())));

            return new AcessoDTO()
            {
                data = data.Select(meta =>
                {
                    return (meta.DATA_COVER_SMALL == null || meta.DATA_COVER_LARGE == null) ?
                            itens.Where(item => item.PACKAGE_NAME == meta.PACKAGE_NAME.ToString()).Select(item => { item.ACESSO = meta.ACESSO; return item; }).FirstOrDefault() :
                            new AcessoDTO.Result() {
                                ACESSO = meta.ACESSO,
                                DATA_COVER_LARGE = meta.DATA_COVER_LARGE,
                                DATA_COVER_SMALL = meta.DATA_COVER_SMALL,
                                DESCRICAO = meta.DESCRICAO,
                                PACKAGE_NAME = meta.PACKAGE_NAME
                            };
                }).ToList(),
                sucesso = query.ReadSingle<SucessoDTO>()
            };
            */


            return new AcessoDTO()
            {
                data = query.Read<AcessoDTO.Result>().ToList(),
                sucesso = query.ReadSingle<SucessoDTO>()
            };
        }

        public static List<string> SendSMS()
        {
            return Utils.DapperConnection.Query(" SELECT TELEFONE FROM NUMERO_TELEFONE ").Select(item => (string)item.TELEFONE).ToList();
        }

        public static List<AppMetadata> getAppList(string name, string androidId)
        {
            //List<AppMetadata> retorno = getAppListByRegex(name,
            //    "<img.+?src=[\"'](.+?)[\"'].+?>|<span class=\"preview-overlay-container\" data-docid=[\"'](.+?)[\"'].+?>",
            //    "https://play.google.com/store/search?c=apps&q=");

            //List<AppMetadata> retornoDB = getApps(retorno.Select(x => x.PackageName).ToList(), androidId);
            //return retornoDB.Union(retorno, new AppMetadataComparer()).ToList();

            List<AppMetadata> retorno = new List<AppMetadata>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UNICELLLOCKER"].ConnectionString))
            {
                con.Open();
                try
                {
                    using (SqlCommand cmd = new SqlCommand(" SELECT A.ID, A.PACKAGE_NAME, A.DESCRICAO, A.DATA_COVER_SMALL, A.DATA_COVER_LARGE, " +
                                                            " CASE WHEN AP.ID_APP IS NULL THEN('N') ELSE 'S' END AS AUTORIZADO " +
                                                            " FROM APP A " +
                                                            " LEFT JOIN APP_AUTORIZADO AP ON A.ID = AP.ID_APP AND AP.ID_MOBILE = @ANDROID_ID " +
                                                            " WHERE A.PACKAGE_NAME LIKE @PACKAGE_NAME OR AP.ID_APP is not null", con))
                    {

                        cmd.Parameters.AddWithValue("@PACKAGE_NAME", "%" + name + "%");
                        cmd.Parameters.AddWithValue("@ANDROID_ID", androidId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                retorno.Add(new AppMetadata()
                                {
                                    Id = reader.GetInt32(0),
                                    PackageName = reader.GetString(1),
                                    Descricao = reader.GetString(2),
                                    dataCoverSmall = reader.GetString(3),
                                    dataCoverLarge = reader.GetString(4),
                                    Autorizado = reader.GetString(5)
                                });
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    return retorno;
                }
                finally
                {
                    con.Close();
                }
            }

            return retorno;
        }

        public static List<AppMetadataDTO> getApps(List<string> packageNames, string androidId)
        {
            var retorno = Utils.DapperConnection.Query(" SELECT A.ID, A.PACKAGE_NAME, A.DESCRICAO, A.DATA_COVER_SMALL, A.DATA_COVER_LARGE, " +
                                                            " AP.AUTORIZADO " +
                                                            " FROM APP A " +
                                                            " LEFT JOIN APP_AUTORIZADO AP ON A.ID = AP.ID_APP AND AP.ID_MOBILE = @ANDROID_ID " +
                                                            " WHERE AP.ID_APP IS NOT NULL OR(AP.ID_APP IS NULL AND A.PACKAGE_NAME IN(@PACKAGE_NAME))",
                                                            new
                                                            {
                                                                PACKAGE_NAME = string.Join(",", packageNames),
                                                                ANDROID_ID = androidId
                                                            }).Select(
            item => new AppMetadataDTO()
            {
                Id = item.ID,
                PackageName = item.PACKAGE_NAME,
                Descricao = item.DESCRICAO,
                dataCoverSmall = item.DATA_COVER_SMALL,
                dataCoverLarge = item.DATA_COVER_LARGE,
                Autorizado = item.AUTORIZADO
            }).ToList();

            return retorno;
        }

        public static CargoDTO GetCargo(int? ID_EMPRESA, int? QtdPorPagina, int? Pagina, string search)
        {
            var query = Utils.DapperConnection.QueryMultiple("GET_CARGO", new
            {
                ID_EMPRESA = ID_EMPRESA,
                QtdPorPagina = QtdPorPagina,
                Pagina = Pagina,
                search = search,
            }, commandType: CommandType.StoredProcedure);

            return new CargoDTO()
            {
                data = query.Read<CargoDTO.Result>().ToList(),
                sucesso = query.ReadFirst<SucessoDTO>()
            };
        }

        public static FuncionarioDTO GetFuncionario(int? ID_EMPRESA, int? QtdPorPagina, int? Pagina, string search)
        {
            var query = Utils.DapperConnection.QueryMultiple("GET_FUNCIONARIO", new
            {
                ID_EMPRESA = ID_EMPRESA,
                QtdPorPagina = QtdPorPagina,
                Pagina = Pagina,
                search = search,
            }, commandType: CommandType.StoredProcedure);

            return new FuncionarioDTO()
            {
                data = query.Read<FuncionarioDTO.Result>().ToList(),
                sucesso = query.ReadFirst<SucessoDTO>()
            };
        }

        public static SucessoDTO SetCargo(int? ID_EMPRESA, string NM_CARGO, int? ID)
        {
            var query = Utils.DapperConnection.QueryFirst<SucessoDTO>("SET_CARGO", new
            {
                ID_EMPRESA = ID_EMPRESA,
                NM_CARGO = NM_CARGO,
                ID = ID,
            }, commandType: CommandType.StoredProcedure);

            return query;
        }

        public static SucessoDTO SetFuncionario(int? ID_EMPRESA, string NM_FUNCIONARIO, int? ID, int? ID_CARGO, string CPF, string RG, bool? GENERO)
        {
            var query = Utils.DapperConnection.QueryFirst<SucessoDTO>("SET_FUNCIONARIO", new
            {
                ID_EMPRESA = ID_EMPRESA,
                NM_FUNCIONARIO = NM_FUNCIONARIO,
                ID = ID,
                ID_CARGO = ID_CARGO,
                CPF = CPF,
                RG = RG,
                GENERO = GENERO,
            }, commandType: CommandType.StoredProcedure);

            return query;
        }
    }
}
