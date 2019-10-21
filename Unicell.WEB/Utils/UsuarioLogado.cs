using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Unicell.DTO;

namespace Mirante.Util
{
    public class UsuarioLogado
    {
        public static UsuarioDTO Usuario
        {
            get
            {
                try
                {
                    var cookie = Utility.Cookies.Get(Utility.Cookies.USUARIO);

                    dynamic usuario = new JavaScriptSerializer().DeserializeObject(cookie);

                    return new UsuarioDTO()
                    {
                        UserName = usuario["UserName"],
                        UserID = usuario["UserID"] ?? 0,
                        ID_Empresa = usuario["ID_Empresa"] ?? 0
                    };
                }
                catch (Exception ex)
                {
                    return new UsuarioDTO() { UserID = 0, ID_Empresa = 0 };
                }
            }
            set
            {

                Utility.Cookies.New(Utility.Cookies.USUARIO, new JavaScriptSerializer().Serialize(value));
            }
        }
    }
}