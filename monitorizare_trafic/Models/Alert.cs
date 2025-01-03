using System;
using System.Data.Linq.Mapping;

namespace monitorizare_trafic.Models
{
    [Table(Name = "Alerts")] // Atributul pentru a mapa la tabela "Alerts" din baza de date
    public class Alert
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] // Coloana primară generată automat
        public int AlertID { get; set; }

        [Column] // Coloana "Timestamp"
        public DateTime Timestamp { get; set; } = DateTime.Now; // Setează valoarea implicită la data și ora curentă

        [Column] // Coloana "TrafficID"
        public int TrafficID { get; set; }

        [Column] // Coloana "AlertType"
        public string AlertType { get; set; }

        [Column] // Coloana "Message"
        public string Message { get; set; }

        [Column] // Coloana "Resolved"
        public bool Resolved { get; set; } = false; // Valoare implicită la false
    }
}
