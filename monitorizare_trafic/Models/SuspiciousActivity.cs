using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitorizare_trafic.Models
{
    public class SuspiciousActivity
    {
        public int Id { get; set; }
        public string ActivityType { get; set; }
        public DateTime DetectedAt { get; set; }
        public string Details { get; set; }
    }
}
