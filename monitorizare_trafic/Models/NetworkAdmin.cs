using System;
using System.Collections.Generic;
using monitorizare_trafic.Utils;
using monitorizare_trafic.Models;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;

namespace monitorizare_trafic.Models
{
    public class NetworkAdmin : User
    {
        private Manager _manager;

        public NetworkAdmin()
        {
            _manager = new Manager();
        }
        public void AddAddressToList(AddressListEntry entry)
        {
            Manager manager = new Manager();
            using (var context = manager.GetDataContext())
            {
                context.GetTable<AddressListEntry>().InsertOnSubmit(entry);
                context.SubmitChanges();
            }
        }
        public List<AddressListEntry> GetAddresses(string listType)
        {
            Manager manager = new Manager();
            using (var context = manager.GetDataContext())
            {
                return context.GetTable<AddressListEntry>()
                    .Where(a => a.ListType == listType)
                    .ToList();
            }
        }

        // Funcție pentru vizualizarea tuturor utilizatorilor
        public List<User> GetUsers()
        {
            return _manager.GetAllUsers();
        }
        public ObservableCollection<NetworkPort> GetActivePorts()
        {
            // Simulate getting active ports - replace with actual implementation
            return new ObservableCollection<NetworkPort>
        {
            new NetworkPort { PortNumber = 80, Status = "Open", Service = "HTTP", Protocol = "TCP" },
            new NetworkPort { PortNumber = 443, Status = "Open", Service = "HTTPS", Protocol = "TCP" },
            // Add more ports as needed
        };
        }
        public void RemoveAddressFromList(AddressListEntry entry)
        {
            Manager manager = new Manager();
            using (var context = manager.GetDataContext())
            {
                var addressToRemove = context.GetTable<AddressListEntry>()
                    .FirstOrDefault(a => a.Id == entry.Id);

                if (addressToRemove != null)
                {
                    context.GetTable<AddressListEntry>().DeleteOnSubmit(addressToRemove);
                    context.SubmitChanges();
                }
            }
        }



        // Funcție pentru adăugarea unui utilizator
        public void AddUser(User newUser)
        {
            _manager.AddUser(newUser);
            Console.WriteLine("Utilizatorul a fost adăugat în baza de date.");
        }

        // Funcție pentru ștergerea unui utilizator
        public void RemoveUser(int userId)
        {
            _manager.RemoveUser(userId);
            Console.WriteLine("Utilizatorul a fost șters din baza de date.");
        }

        // Funcție pentru blocarea unui port
        public void BlockPort(int portNumber)
        {
            // Logica pentru blocarea portului
            Console.WriteLine($"Portul {portNumber} a fost blocat.");
        }

        // Funcție pentru activarea protecției
        public void EnableProtection()
        {
            // Logica pentru activarea protecției
            Console.WriteLine("Sistemul de protecție a fost activat.");
        }

        // Funcție pentru dezactivarea protecției
        public void DisableProtection()
        {
            // Logica pentru dezactivarea protecției
            Console.WriteLine("Sistemul de protecție a fost dezactivat.");
        }
    }
}
