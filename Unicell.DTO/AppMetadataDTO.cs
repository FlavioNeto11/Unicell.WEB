using System;
using System.Collections.Generic;
using System.Text;

namespace Unicell.DTO
{
    public class AppMetadataDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string PackageName { get; set; }
        public string Acesso { get; set; }
        public string dataCoverSmall { get; set; }
        public string dataCoverLarge { get; set; }
        public string Autorizado { get; set; }
    }
}
