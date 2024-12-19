﻿using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitorizare_trafic.Models
{

    [Table(Name = "Reports")]
    public class Report
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column]
        public string Title { get; set; } // Numele raportului
        [Column]
        public string Description { get; set; }
        [Column]
        public string Category { get; set; } // Categoria
        [Column]
        public DateTime CreatedDate { get; set; } // Data evenimentului
        [Column]
        public int CreatedBy { get; set; }
        [Column]
        public string Status {  get; set; }
        [Column]
        public int Priority { get; set; } // Nivelul de prioritate (1-5)
        
    }


}