using System;
using System.Data.Linq.Mapping;

namespace monitorizare_trafic.Models
{
    [Table(Name = "NetworkData")] 
    public class NetworkData
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] 
        public int Id { get; set; }

        [Column] 
        public string SourceIP { get; set; }

        [Column] 
        public string DestinationIP { get; set; }

        [Column] 
        public int DataSize { get; set; }

        [Column] 
        public int Port { get; set; }

        [Column] 
        public DateTime Timestamp { get; set; } = DateTime.Now; 
    }
}
