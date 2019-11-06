using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Unicell.DTO
{
    public class AcessoDTO : MasterDTO<AcessoDTO.Result>
    {
        public class Result
        {
            public string PACKAGE_NAME { get; set; }
            public DateTime ACESSO { get; set; }
            public string ACESSO_STRING { get { return ACESSO.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("pt-BR")); } }
            public string DATA_COVER_SMALL { get; set; }
            public string DATA_COVER_LARGE { get; set; }
            public string DESCRICAO { get; set; }
        }
    }
}
