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
            
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please fill all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string hashedPassword = SecurityHelper.ComputeHash(txtPassword.Password);

            User newUser = new User
            {
                Username = txtUsername.Text,
                Password = hashedPassword,  
                Role = (cmbRole.SelectedItem as ComboBoxItem).Content.ToString(),
                Email = txtEmail.Text
            };

            try
            {
               
                Manager manager = new Manager();
                DataContext db = manager.GetDataContext();
                db.GetTable<User>().InsertOnSubmit(newUser);
                db.SubmitChanges();

               
                MessageBox.Show("User added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }
    }
}
