using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitorizare_trafic.Models
{
    internal class NetworkData
    {
        public string SourceIP { get; set; }
        public string DestinationIP { get; set; }
        public int DataSize { get; set; }
        public int Port { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
