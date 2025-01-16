using System;
using System.Windows;
using monitorizare_trafic.Models;
using System.Linq;
using monitorizare_trafic.Utils;
using System.Data.Linq;

namespace monitorizare_trafic.View
{
    public partial class DeleteUserDialog : Window
    {
        public DeleteUserDialog()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
         
            Manager manager = new Manager();
            DataContext db = manager.GetDataContext();
            var userToDelete = db.GetTable<User>().FirstOrDefault(u => u.Username == txtUsername.Text);

            if (userToDelete != null)
            {
                db.GetTable<User>().DeleteOnSubmit(userToDelete);
                db.SubmitChanges();
                MessageBox.Show("User deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("User not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
