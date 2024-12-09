using System;
using System.Data.Linq;
using System.Windows;
using monitorizare_trafic.Models;
using monitorizare_trafic.Utils;

namespace monitorizare_trafic.View
{
    public partial class AddUserDialog : Window
    {
        public AddUserDialog()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Calculăm hash-ul parolei
            string hashedPassword = SecurityHelper.ComputeHash(txtPassword.Password);

            // Adăugăm utilizatorul în baza de date
            User newUser = new User
            {
                Username = txtUsername.Text,
                Password = hashedPassword,  // Folosim hash-ul calculat
                Role = txtRole.Text,
                Email = txtEmail.Text
            };

            // Salvarea în baza de date (folosind managerul de conexiune)
            Manager manager = new Manager();
            DataContext db = manager.GetDataContext();
            db.GetTable<User>().InsertOnSubmit(newUser);
            db.SubmitChanges();

            MessageBox.Show("User added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
