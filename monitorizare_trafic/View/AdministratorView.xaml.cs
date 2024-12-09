using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using monitorizare_trafic.Models;

namespace monitorizare_trafic.View
{
    public partial class AdministratorView : Window
    {
        private NetworkAdmin networkAdmin;

        public AdministratorView()
        {
            InitializeComponent();
            networkAdmin = new NetworkAdmin();
            LoadUsers(); // Încarcă utilizatorii la inițializare
        }

        // Încărcare utilizatori în DataGrid
        private void LoadUsers()
        {
            try
            {
                var users = networkAdmin.GetUsers(); // Obține lista utilizatorilor
                UserDataGrid.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}");
            }
        }

        // Buton pentru vizualizarea utilizatorilor
        private void btnViewUsers_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers(); // Reîncarcă utilizatorii
            MessageBox.Show("User list refreshed.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Buton pentru adăugarea unui utilizator
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUserDialog addUserDialog = new AddUserDialog
            {
                Owner = this // Setăm fereastra principală ca părinte
            };

            if (addUserDialog.ShowDialog() == true)
            {
                LoadUsers(); // Reîncarcă utilizatorii după adăugare
            }
        }

        // Buton pentru ștergerea unui utilizator
        private void btnRemoveUser_Click(object sender, RoutedEventArgs e)
        {
            if (UserDataGrid.SelectedItem is User selectedUser)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {selectedUser.Username}?",
                                             "Delete Confirmation",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        networkAdmin.RemoveUser(selectedUser.UserId); // Ștergere utilizator
                        LoadUsers(); // Reîncarcă utilizatorii
                        MessageBox.Show("User deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting user: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("No user selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Gestionare ștergere utilizator din ContextMenu
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            btnRemoveUser_Click(sender, e); // Folosește logica butonului
        }

        // Buton pentru blocarea unui port
        private void btnBlockPort_Click(object sender, RoutedEventArgs e)
        {
            networkAdmin.BlockPort(8080); // Exemplu cu portul 8080
            MessageBox.Show("Port 8080 blocked.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Buton pentru activarea protecției
        private void btnEnableProtection_Click(object sender, RoutedEventArgs e)
        {
            networkAdmin.EnableProtection();
            MessageBox.Show("Protection enabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Buton pentru dezactivarea protecției
        private void btnDisableProtection_Click(object sender, RoutedEventArgs e)
        {
            networkAdmin.DisableProtection();
            MessageBox.Show("Protection disabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
