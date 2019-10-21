using System;
using System.Collections.Generic;
using System.Text;

namespace Unicell.DTO
{
    public class FuncionarioDTO : MasterDTO<FuncionarioDTO.Result>
    {
        public class Result
        {
            public int? ID_FUNCIONARIO { get; set; }
            public string NM_FUNCIONARIO { get; set; }
            public string NM_EMPRESA { get; set; }
            public int? ID_EMPRESA { get; set; }
            public int? ID_CARGO { get; set; }
            public string NM_CARGO { get; set; }
            public string CPF { get; set; }
            public string RG { get; set; }
            public bool? GENERO { get; set; }
        }
    }
}
