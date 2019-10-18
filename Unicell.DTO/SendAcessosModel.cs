using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unicell.API.Models
{
    public class SendAcessosModel
    {
        public string androidID { get; set; }
        public List<Acesso> packages { get; set; }

        public class Acesso
        {
            public string packageName { get; set; }
            public string acesso { get; set; }
        }
    }
}