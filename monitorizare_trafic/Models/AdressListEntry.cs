using System;
using System.Data.Linq.Mapping;

namespace monitorizare_trafic.Models
{
    [Table(Name = "AddressList")] 
    public class AddressListEntry
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; } 

        [Column(Name = "Address")]
        public string Address { get; set; }

        [Column(Name = "Type")]
        public string Type { get; set; } 

        [Column(Name = "ListType")]
        public string ListType { get; set; }

        [Column(Name = "DateAdded")]
        public DateTime DateAdded { get; set; }

        [Column(Name = "Description", CanBeNull = true)]
        public string Description { get; set; } 
    }
}
