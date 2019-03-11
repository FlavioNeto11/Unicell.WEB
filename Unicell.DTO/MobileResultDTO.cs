using System;
using System.Collections.Generic;
using System.Text;

namespace Unicell.DTO
{
    public class MobileResultDTO
    {
        public List<string> AppAutorizado { get; set; } = new List<string>();
        public int Versao { get; set; }
        public bool Icon { get; set; }
    }
}
