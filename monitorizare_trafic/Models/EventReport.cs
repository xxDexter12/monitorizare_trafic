using System;
using System.Data.Linq.Mapping;

namespace monitorizare_trafic.Models
{
    [Table(Name = "EventReports")]
    public class EventReport
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int EventReportId { get; set; }

        [Column]
        public int ReportId { get; set; }  

        [Column]
        public string AnalystComments { get; set; }

        [Column]
        public string SuspiciousPackets { get; set; } 

        [Column]
        public DateTime CreatedDate { get; set; }

        [Column]
        public int AnalystId { get; set; }  
    }
}
