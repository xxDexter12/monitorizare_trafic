using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitorizare_trafic.Utils
{
    public class Manager
    {
        public string GetConnectionString()
        {
            
            return "Server=DESKTOP-UMU84AP\\SQLEXPRESS;Database=IDSDB;Trusted_Connection=True;";//DESKTOP-MJ5QMKE\SQLEXPRESS , DESKTOP-UMU84AP\\SQLEXPRESS
        }

        public DataContext GetDataContext()
        {
            string connectionString = GetConnectionString();
            return new DataContext(connectionString);   

        }
    }
}
