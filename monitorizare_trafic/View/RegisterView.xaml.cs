using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using monitorizare_trafic.Models;
using monitorizare_trafic.Utils;

namespace monitorizare_trafic.View
{
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Password;
            string email = txtEmail.Text;

            // Verificăm dacă toate câmpurile sunt completate
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Validarea email-ului (simplu, folosind o expresie regulată)
            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(email))
            {
                MessageBox.Show("Please enter a valid email address.");
                return;
            }

            string passwordHash = SecurityHelper.ComputeHash(password); // Calculăm hash-ul parolei

            // Creează instanța clasei Manager pentru a obține contextul de date
            var manager = new Manager();

            // Obține contextul de date
            using (var db = manager.GetDataContext())
            {
                // Verificăm dacă utilizatorul sau email-ul există deja
                var existingUser = db.GetTable<User>().FirstOrDefault(u => u.Username == username || u.Email == email);

                if (existingUser != null)
                {
                    MessageBox.Show("Username or Email already exists.");
                }
                else
                {
                    // Creăm un nou utilizator și îl salvăm în baza de date
                    var newUser = new User
                    {
                        Username = username,
                        Password = passwordHash,
                        Email = email,  // Adăugăm email-ul
                        Role = "User"  // Setează un rol de bază (poți modifica în funcție de nevoile tale)
                    };

                    db.GetTable<User>().InsertOnSubmit(newUser);
                    db.SubmitChanges();

                    MessageBox.Show("Registration successful!");
                    var loginWindow = new LoginView();
                    loginWindow.Show();
                    this.Close(); // Închide fereastra de înregistrare
                }
            }
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginView(); // Crează o instanță a ferestrei de login
            loginWindow.Show(); // Deschide fereastra
            this.Close();
        }
    }
}
