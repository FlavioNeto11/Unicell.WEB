using Mirante.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Unicell.WEB.Utils
{
    public class Autorizacao : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //Caso não encontre um usuário logado
            if (UsuarioLogado.Usuario == null || UsuarioLogado.Usuario.UserID == null || UsuarioLogado.Usuario.UserID == 0)
                return false;

            //Limpa todos os Caches que houverem
            foreach (System.Collections.DictionaryEntry entry in HttpContext.Current.Cache)
            {
                HttpContext.Current.Cache.Remove((string)entry.Key);
            }

            //Caso haja um usuário logado, autoriza o acesso
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Login/Logout");
        }
    }
}