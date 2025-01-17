﻿using System;
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

  
        public List<User> GetUsers()
        {
            return _manager.GetAllUsers();
        }
        public ObservableCollection<NetworkPort> GetActivePorts()
        {
  
            return new ObservableCollection<NetworkPort>
        {
            new NetworkPort { PortNumber = 80, Status = "Open", Service = "HTTP", Protocol = "TCP" },
            new NetworkPort { PortNumber = 443, Status = "Open", Service = "HTTPS", Protocol = "TCP" },

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

        public List<EventReport> GetEventReports()
        {
            Manager manager = new Manager();
            using (var context = manager.GetDataContext())
            {
                return context.GetTable<EventReport>().ToList();
            }
        }



    
        public void AddUser(User newUser)
        {
            _manager.AddUser(newUser);
            Console.WriteLine("Utilizatorul a fost adăugat în baza de date.");
        }

     
        public void RemoveUser(int userId)
        {
            _manager.RemoveUser(userId);
            Console.WriteLine("Utilizatorul a fost șters din baza de date.");
        }

     
        public void BlockPort(int portNumber)
        {

            Console.WriteLine($"Portul {portNumber} a fost blocat.");
        }

 
        public void EnableProtection()
        {

            Console.WriteLine("Sistemul de protecție a fost activat.");
        }


        public void DisableProtection()
        {

            Console.WriteLine("Sistemul de protecție a fost dezactivat.");
        }
    }
}
