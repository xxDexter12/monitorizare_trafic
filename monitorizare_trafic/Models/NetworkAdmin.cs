using System;
using System.Collections.Generic;
using monitorizare_trafic.Utils;
using monitorizare_trafic.Models;

namespace monitorizare_trafic.Models
{
    public class NetworkAdmin : User
    {
        private Manager _manager;

        public NetworkAdmin()
        {
            _manager = new Manager();
        }

        // Funcție pentru vizualizarea tuturor utilizatorilor
        public List<User> GetUsers()
        {
            return _manager.GetAllUsers();
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
