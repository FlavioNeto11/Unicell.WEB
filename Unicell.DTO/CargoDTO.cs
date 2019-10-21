using System;
using System.Collections.Generic;
using System.Text;

namespace Unicell.DTO
{
    public class CargoDTO : MasterDTO<CargoDTO.Result>
    {
        public class Result
        {
            public int? ID_CARGO { get; set; }
            public string NM_CARGO { get; set; }
            public string NM_EMPRESA { get; set; }
            public int? ID_EMPRESA { get; set; }
        }
    }
}
