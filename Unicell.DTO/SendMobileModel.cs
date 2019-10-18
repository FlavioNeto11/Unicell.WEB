using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unicell.API.Models
{
    public class SendMobileModel
    {
        public string androidID { get; set; }
        public string geoLocation { get; set; }
        public string androidStatus { get; set; }
        public string[] telefones { get; set; }
        public int signalStrength { get; set; }
        public bool isCharging { get; set; }
        public float chargeLevel { get; set; }
    }
}