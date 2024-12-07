using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using monitorizare_trafic.Models;
using monitorizare_trafic.Utils;
using monitorizare_trafic.Models;
namespace monitorizare_trafic.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string passwordHash = SecurityHelper.ComputeHash(txtPass.Password);  // Calculăm hash-ul parolei introduse

            // Creează instanța clasei Manager pentru a obține contextul de date
            var manager = new Manager();

            // Obține contextul de date
            using (var db = manager.GetDataContext())
            {
                // Verificăm utilizatorul în baza de date
                var user = db.GetTable<User>().FirstOrDefault(u => u.Username == username && u.Password == passwordHash);

                if (user != null)
                {
                    MessageBox.Show($"Logat ca {user.Role}.");

                    // Navigare în funcție de rolul utilizatorului
                    switch (user.Role)
                    {
                        case "Admin":
                            //var adminWindow = new AdminView(); // Fereastra pentru Admin
                            //adminWindow.Show();
                            this.Close(); // Închide fereastra de login
                            break;

                        case "Analyst":
                            //var analystWindow = new AnalystView(); // Fereastra pentru Analyst
                            //analystWindow.Show();
                            this.Close(); // Închide fereastra de login
                            break;

                        case "User":
                            var userWindow = new UserView(); // Fereastra pentru BasicUser
                            userWindow.Show();
                            this.Close(); // Închide fereastra de login
                            break;

                        default:
                            MessageBox.Show("Rol necunoscut.");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Credențiale incorecte.");
                }
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                btnLogin_Click(sender, e); // Apelăm funcția de login
            }
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Mută focusul la următorul control (de exemplu, PasswordTextBox)
                txtPass.Focus();
            }
        }
        private void RegisterButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var registerWindow = new RegisterView(); // Crează o instanță a ferestrei RegisterView
            registerWindow.Show(); // Deschide fereastra de înregistrare
            this.Close(); // Închide fereastra de login dacă dorești
        }

    }
}