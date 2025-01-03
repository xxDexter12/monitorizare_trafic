using System;
using System.Data.Linq.Mapping;

namespace monitorizare_trafic.Models
{
    [Table(Name = "NetworkData")] // Atributul pentru a mapa la tabela "NetworkData" din baza de date
    public class NetworkData
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] // Coloana primară generată automat
        public int Id { get; set; }

        [Column] // Coloana "SourceIP"
        public string SourceIP { get; set; }

        [Column] // Coloana "DestinationIP"
        public string DestinationIP { get; set; }

        [Column] // Coloana "DataSize"
        public int DataSize { get; set; }

        [Column] // Coloana "Port"
        public int Port { get; set; }

        [Column] // Coloana "Timestamp"
        public DateTime Timestamp { get; set; } = DateTime.Now; // Setează valoarea implicită la data și ora curentă
    }
}
