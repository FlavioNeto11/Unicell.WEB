using System;
using System.Collections.Generic;
using System.Text;

namespace Unicell.DTO
{
    public class MobileResultDTO
    {
        public List<Autorizacao> Autorizacoes { get; set; } = new List<Autorizacao>();
        public Configuracoes Configuracao { get; set; } = new Configuracoes();

        public class Autorizacao
        {
            public string PACKAGE_NAME { get; set; }
            public bool? AUTORIZADO { get; set; }
        }

        public class Configuracoes
        {
            public bool? ICON { get; set; }
            public bool? ACESSO_IRRESTRITO { get; set; }
            public bool? ACESSO_LOJA { get; set; }
            public bool? ACESSO_CONFIGURACAO { get; set; }
            public int? ID_EMPRESA { get; set; }
        }
    }
}
