using System;

namespace Unicell.DTO
{
    public class MobileDTO : MasterDTO<MobileDTO.Result>
    {
        public class Result
        {
            public string ANDROID_ID { get; set; }
            public string GEO_LOCALIZACAO { get; set; }
            public DateTime ULTIMO_ACESSO { get; set; }
            public string ANDROID_STATUS { get; set; }
            public string NM_FUNCIONARIO { get; set; }
            public bool? ICON { get; set; }
        }
    }
}
