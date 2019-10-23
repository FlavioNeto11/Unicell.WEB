using System;

namespace Unicell.DTO
{
    public class MobileDTO : MasterDTO<MobileDTO.Result>
    {
        public class Result
        {
            public string ANDROID_ID { get; set; }
            public string GEO_LOCALIZACAO { get; set; }
            public string ENDERECO { get; set; }
            public DateTime ULTIMO_ACESSO { get; set; }
            public string ULTIMO_ACESSO_STRING { get { return (ULTIMO_ACESSO != null) ? ULTIMO_ACESSO.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty; } }
            public string ANDROID_STATUS { get; set; }
            public string NM_FUNCIONARIO { get; set; }
            public bool? ICON { get; set; }
            public bool? ISCHARGING { get; set; }
            public int? SIGNALSTRENGTH { get; set; }
            public float? CHARGELEVEL { get; set; }
        }
    }
}
