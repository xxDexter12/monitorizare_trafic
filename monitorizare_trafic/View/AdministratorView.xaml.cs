using System;
using System.Windows;
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
        }

        // Buton pentru vizualizarea utilizatorilor
        private void btnViewUsers_Click(object sender, RoutedEventArgs e)
        {
            var users = networkAdmin.GetUsers(); // Obține lista de utilizatori
            UserDataGrid.ItemsSource = users;
            MessageBox.Show($"Numărul de utilizatori: {users.Count}");
        }

        // Buton pentru adăugarea unui utilizator
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUserDialog addUserDialog = new AddUserDialog();
            addUserDialog.Owner = this;  // Setăm fereastra principală ca părinte
            addUserDialog.ShowDialog();  // Afișăm fereastra ca un dialog modal
        }

        // Buton pentru ștergerea unui utilizator
        private void btnRemoveUser_Click(object sender, RoutedEventArgs e)
        {
            DeleteUserDialog deleteUserDialog = new DeleteUserDialog();
            deleteUserDialog.Owner = this;  // Setăm fereastra principală ca părinte
            deleteUserDialog.ShowDialog();  // Afișăm fereastra ca un dialog modal
        }

        // Buton pentru blocarea unui port
        private void btnBlockPort_Click(object sender, RoutedEventArgs e)
        {
            networkAdmin.BlockPort(8080); // Exemplu cu portul 8080
        }

        // Buton pentru activarea protecției
        private void btnEnableProtection_Click(object sender, RoutedEventArgs e)
        {
            networkAdmin.EnableProtection();
        }

        // Buton pentru dezactivarea protecției
        private void btnDisableProtection_Click(object sender, RoutedEventArgs e)
        {
            networkAdmin.DisableProtection();
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
