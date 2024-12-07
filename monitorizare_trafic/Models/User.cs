using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitorizare_trafic.Models
{
    [Table(Name = "Users")] // Atributul pentru a mapa la tabela "Users" din baza de date
    public class User
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] // Coloana primară generată automat
        public int UserId { get; set; }

        [Column] // Coloana "UserName"
        public string Username { get; set; }

        [Column] // Coloana "PasswordHash"
        public string Password { get; set; }

        [Column] // Coloana "Role"
        public string Role { get; set; }

        [Column]
        public string Email { get; set; }
    }
}
