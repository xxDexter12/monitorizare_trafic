﻿using System;
using System.Data.Linq.Mapping;

namespace monitorizare_trafic.Models
{
    [Table(Name = "EventReports")]
    public class EventReport
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int EventReportId { get; set; }

        [Column]
        public int ReportId { get; set; }  // Foreign key to Report

        [Column]
        public string AnalystComments { get; set; }

        [Column]
        public string SuspiciousPackets { get; set; }  // Store as JSON or comma-separated string

        [Column]
        public DateTime CreatedDate { get; set; }

        [Column]
        public int AnalystId { get; set; }  // ID of the analyst who created this
    }
}
