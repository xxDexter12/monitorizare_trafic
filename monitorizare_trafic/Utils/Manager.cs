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
            
            return "Server=DESKTOP-MJ5QMKE\\SQLEXPRESS;Database=IDSDB;Trusted_Connection=True;";//DESKTOP-MJ5QMKE\SQLEXPRESS , DESKTOP-UMU84AP\\SQLEXPRESS
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

        // Metodă pentru a adăuga un utilizator
        public void AddUser(User newUser)
        {
            using (var context = GetDataContext())
            {
                context.GetTable<User>().InsertOnSubmit(newUser);
                context.SubmitChanges(); // Salvează modificările în baza de date
            }
        }

        // Metodă pentru a șterge un utilizator
        public void RemoveUser(int userId)
        {
            using (var context = GetDataContext())
            {
                var userToRemove = context.GetTable<User>().FirstOrDefault(u => u.UserId == userId);
                if (userToRemove != null)
                {
                    context.GetTable<User>().DeleteOnSubmit(userToRemove);
                    context.SubmitChanges(); // Salvează modificările în baza de date
                }
            }
        }

    }
}
