using System;

namespace monitorizare_trafic.Models
{
    public class TrafficTrend
    {
        public DateTime Timestamp { get; set; }
        public int PacketCount { get; set; }
    }
}
