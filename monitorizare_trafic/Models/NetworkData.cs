using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitorizare_trafic.Models
{
    public class NetworkData
    {
        public string SourceIp { get; set; }
        public string DestinationIp { get; set; }
        public int Port { get; set; }
        public int DataSize { get; set; }
        public DateTime Timestamp { get; set; }=DateTime.Now;
    }
}
