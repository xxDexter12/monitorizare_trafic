using System;
using System.Data.Linq.Mapping;

namespace monitorizare_trafic.Models
{
    [Table(Name = "Alerts")] 
    public class Alert
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] 
        public int AlertID { get; set; }

        [Column] 
        public DateTime Timestamp { get; set; } = DateTime.Now; 

        [Column]
        public int TrafficID { get; set; }

        [Column] 
        public string AlertType { get; set; }

        [Column]
        public string Message { get; set; }

        [Column]
        public bool Resolved { get; set; } = false;
    }
}
