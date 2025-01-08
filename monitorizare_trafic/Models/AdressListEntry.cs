using System;
using System.Data.Linq.Mapping;

namespace monitorizare_trafic.Models
{
    [Table(Name = "AddressList")] // Specifică tabela din baza de date
    public class AddressListEntry
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] // Coloana primară generată automat
        public int Id { get; set; } // ID unic

        [Column(Name = "Address")]
        public string Address { get; set; } // Adresa IP sau domeniul

        [Column(Name = "Type")]
        public string Type { get; set; } // Tipul: IP Address sau Domain

        [Column(Name = "ListType")]
        public string ListType { get; set; } // Whitelist sau Blacklist

        [Column(Name = "DateAdded")]
        public DateTime DateAdded { get; set; } // Data adăugării

        [Column(Name = "Description", CanBeNull = true)]
        public string Description { get; set; } // Descriere sau motiv (opțional)
    }
}
