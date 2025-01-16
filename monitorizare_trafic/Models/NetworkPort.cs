using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitorizare_trafic.Models
{
    public class NetworkPort
    {
        public int PortNumber { get; set; }
        public string Status { get; set; }  
        public string Service { get; set; }
        public string Protocol { get; set; } // TCP/UDP
    }
}
