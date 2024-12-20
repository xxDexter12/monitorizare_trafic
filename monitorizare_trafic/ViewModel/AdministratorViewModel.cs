using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using monitorizare_trafic.Models;
using monitorizare_trafic.View;
using monitorizare_trafic.ViewModels.Base;

namespace monitorizare_trafic.ViewModel
{
    public class AdministratorViewModel : ViewModelBase
    {
        private readonly NetworkAdmin _networkAdmin;
        private ObservableCollection<User> _users;
        private User _selectedUser;

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                (RemoveUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand ViewUsersCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand RemoveUserCommand { get; }
        public ICommand BlockPortCommand { get; }
        public ICommand EnableProtectionCommand { get; }
        public ICommand DisableProtectionCommand { get; }
        public ICommand MinimizeCommand { get; }
        public ICommand CloseCommand { get; }

        public AdministratorViewModel()
        {
            _networkAdmin = new NetworkAdmin();

            // Initialize commands
            ViewUsersCommand = new RelayCommand(LoadUsers);
            AddUserCommand = new RelayCommand(AddUser);
            RemoveUserCommand = new RelayCommand(RemoveUser, CanRemoveUser);
            BlockPortCommand = new RelayCommand(BlockPort);
            EnableProtectionCommand = new RelayCommand(EnableProtection);
            DisableProtectionCommand = new RelayCommand(DisableProtection);
            MinimizeCommand = new RelayCommand(Minimize);
            CloseCommand = new RelayCommand(Close);

            // Load users initially
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                var usersList = _networkAdmin.GetUsers();
                Users = new ObservableCollection<User>(usersList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}");
            }
        }

        private void AddUser()
        {
            var addUserDialog = new AddUserDialog
            {
                Owner = Application.Current.MainWindow
            };

            if (addUserDialog.ShowDialog() == true)
            {
                LoadUsers();
            }
        }

        private bool CanRemoveUser()
        {
            return SelectedUser != null;
        }

        private void RemoveUser()
        {
            if (SelectedUser == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete {SelectedUser.Username}?",
                "Delete Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _networkAdmin.RemoveUser(SelectedUser.UserId);
                    LoadUsers();
                    MessageBox.Show("User deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting user: {ex.Message}");
                }
            }
        }

        private void BlockPort()
        {
            _networkAdmin.BlockPort(8080);
            MessageBox.Show("Port 8080 blocked.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EnableProtection()
        {
            _networkAdmin.EnableProtection();
            MessageBox.Show("Protection enabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DisableProtection()
        {
            _networkAdmin.DisableProtection();
            MessageBox.Show("Protection disabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Minimize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Close()
        {
            Application.Current.Shutdown();
        }
    }
}