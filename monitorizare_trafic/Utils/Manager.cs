using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using monitorizare_trafic.Models;

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
        public List<User> GetAllUsers()
        {
            using (var context = GetDataContext())
            {
                return context.GetTable<User>().ToList();
            }
        }

        public List<Report> GetAllReports()
        {
            using (var context=GetDataContext())
            {
                return context.GetTable<Report>().ToList();
            }
        }

      
        public void AddUser(User newUser)
        {
            using (var context = GetDataContext())
            {
                context.GetTable<User>().InsertOnSubmit(newUser);
                context.SubmitChanges(); 
            }
        }

        public void AddNetworkData(NetworkData networkData)
        {
            using (var context=GetDataContext())
            {
                context.GetTable<NetworkData>().InsertOnSubmit(networkData);
                context.SubmitChanges();
            }
        }

   
        public void RemoveUser(int userId)
        {
            using (var context = GetDataContext())
            {
                var userToRemove = context.GetTable<User>().FirstOrDefault(u => u.UserId == userId);
                if (userToRemove != null)
                {
                    context.GetTable<User>().DeleteOnSubmit(userToRemove);
                    context.SubmitChanges(); 
                }
            }
        }

        public void AddEventReport(EventReport eventReport)
        {
            using (var context = GetDataContext())
            {
                context.GetTable<EventReport>().InsertOnSubmit(eventReport);
                context.SubmitChanges();
            }
        }

        public void UpdateReportStatus(int reportId, string newStatus)
        {
            using (var context = GetDataContext())
            {
                var report = context.GetTable<Report>().FirstOrDefault(r => r.ReportId == reportId);
                if (report != null)
                {
                    report.Status = newStatus;
                    context.SubmitChanges();
                }
            }
        }



    }
}
