using System;
using System.Collections.Generic;
using System.Text;

namespace Unicell.DTO
{
    public class SucessoDTO
    {
        public string mensagem { get; set; }
        public bool? sucesso { get; set; }
        public int MaxRows { get; set; }
        public int MaxFilteredRows { get; set; }
        public string wildCard { get; set; }
    }
}
