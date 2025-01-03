using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monitorizare_trafic.Utils;

namespace monitorizare_trafic.Models
{
    public class NetworkAnalyst : User
    {
        private Manager _manager;
        public NetworkAnalyst()
        {
            _manager= new Manager();
        }

        public List<Report> GetReports()
        {
            return _manager.GetAllReports();
        }
    }
}
