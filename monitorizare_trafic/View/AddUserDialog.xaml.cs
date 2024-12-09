using System;
using System.Data.Linq;
using System.Windows;
using System.Windows.Controls;
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
            // Validare date
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please fill all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Calculăm hash-ul parolei
            string hashedPassword = SecurityHelper.ComputeHash(txtPassword.Password);

            // Adăugăm utilizatorul în baza de date
            User newUser = new User
            {
                Username = txtUsername.Text,
                Password = hashedPassword,  // Folosim hash-ul calculat
                Role = (cmbRole.SelectedItem as ComboBoxItem).Content.ToString(),  // Obținem rolul selectat
                Email = txtEmail.Text
            };

            try
            {
                // Salvarea în baza de date (folosind managerul de conexiune)
                Manager manager = new Manager();
                DataContext db = manager.GetDataContext();
                db.GetTable<User>().InsertOnSubmit(newUser);
                db.SubmitChanges();

                // Mesaj de succes
                MessageBox.Show("User added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Închide fereastra după salvarea utilizatorului
            }
            catch (Exception ex)
            {
                // Afișează eroarea în caz de excepție
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Închide fereastra fără a salva datele
            this.Close();
        }
    }
}
