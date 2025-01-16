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
        private ObservableCollection<EventReport> _eventReports;
        private User _selectedUser;
        public User CurrentUser { get; set; }
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        public ObservableCollection<EventReport> EventReports
        {
            get => _eventReports;
            set
            {
                _eventReports = value;
                OnPropertyChanged(nameof(EventReports));
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
        public ICommand AddToWhitelistCommand { get; }
        public ICommand AddToBlacklistCommand { get; }
        public ICommand RemoveFromWhitelistCommand { get; }
        public ICommand RemoveFromBlacklistCommand { get; }
        public ICommand ViewEventReportsCommand { get; }


        public AdministratorViewModel()
        {
            _networkAdmin = new NetworkAdmin();

        
            ViewUsersCommand = new RelayCommand(LoadUsers);
            AddUserCommand = new RelayCommand(AddUser);
            RemoveUserCommand = new RelayCommand(RemoveUser, CanRemoveUser);
            BlockPortCommand = new RelayCommand(BlockPort);
            EnableProtectionCommand = new RelayCommand(EnableProtection);
            DisableProtectionCommand = new RelayCommand(DisableProtection);
            MinimizeCommand = new RelayCommand(Minimize);
            CloseCommand = new RelayCommand(Close);
            AddToWhitelistCommand = new RelayCommand(AddToWhitelist);
            AddToBlacklistCommand = new RelayCommand(AddToBlacklist);
            RemoveFromWhitelistCommand = new RelayCommand(RemoveFromWhitelist);
            RemoveFromBlacklistCommand = new RelayCommand(RemoveFromBlacklist);
            WhitelistedAddresses = new ObservableCollection<AddressListEntry>(_networkAdmin.GetAddresses("Whitelist"));
            BlacklistedAddresses = new ObservableCollection<AddressListEntry>(_networkAdmin.GetAddresses("Blacklist"));
            ViewEventReportsCommand = new RelayCommand(LoadEventReports);

         
            AddToWhitelistCommand = new RelayCommand(AddToWhitelist);
            AddToBlacklistCommand = new RelayCommand(AddToBlacklist);
            RemoveFromWhitelistCommand = new RelayCommand(RemoveFromWhitelist, CanRemoveFromWhitelist);
            RemoveFromBlacklistCommand = new RelayCommand(RemoveFromBlacklist, CanRemoveFromBlacklist);

            
            LoadUsers();
        }

        private void LoadEventReports()
        {
            try
            {
                var reports = _networkAdmin.GetEventReports();
                EventReports = new ObservableCollection<EventReport>(reports);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading event reports: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        private ObservableCollection<NetworkPort> _activePorts; 
        public ObservableCollection<NetworkPort> ActivePorts
        {
            get => _activePorts;
            set
            {
                _activePorts = value;
                OnPropertyChanged(nameof(ActivePorts));
            }
        }
        private ObservableCollection<AddressListEntry> _whitelistedAddresses;
        private ObservableCollection<AddressListEntry> _blacklistedAddresses;

        public ObservableCollection<AddressListEntry> WhitelistedAddresses
        {
            get => _whitelistedAddresses;
            set
            {
                _whitelistedAddresses = value;
                OnPropertyChanged(nameof(WhitelistedAddresses));
            }
        }

        public ObservableCollection<AddressListEntry> BlacklistedAddresses
        {
            get => _blacklistedAddresses;
            set
            {
                _blacklistedAddresses = value;
                OnPropertyChanged(nameof(BlacklistedAddresses));
            }
        }

        private AddressListEntry _selectedWhitelistedAddress;
        private AddressListEntry _selectedBlacklistedAddress;

        public AddressListEntry SelectedWhitelistedAddress
        {
            get => _selectedWhitelistedAddress;
            set
            {
                _selectedWhitelistedAddress = value;
                OnPropertyChanged(nameof(SelectedWhitelistedAddress));
                (RemoveFromWhitelistCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public AddressListEntry SelectedBlacklistedAddress
        {
            get => _selectedBlacklistedAddress;
            set
            {
                _selectedBlacklistedAddress = value;
                OnPropertyChanged(nameof(SelectedBlacklistedAddress));
                (RemoveFromBlacklistCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private bool CanRemoveFromWhitelist()
        {
            return SelectedWhitelistedAddress != null;
        }

        private bool CanRemoveFromBlacklist()
        {
            return SelectedBlacklistedAddress != null;
        }

        private string _newWhitelistAddress;
        public string NewWhitelistAddress
        {
            get => _newWhitelistAddress;
            set
            {
                _newWhitelistAddress = value;
                OnPropertyChanged(nameof(NewWhitelistAddress));
            }
        }

        private string _newBlacklistAddress;
        public string NewBlacklistAddress
        {
            get => _newBlacklistAddress;
            set
            {
                _newBlacklistAddress = value;
                OnPropertyChanged(nameof(NewBlacklistAddress));
            }
        }
        private void AddToWhitelist()
        {
            if (string.IsNullOrWhiteSpace(NewWhitelistAddress))
            {
                MessageBox.Show("Address cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var entry = new AddressListEntry
                {
                    Address = NewWhitelistAddress,
                    Type = "IP Address", 
                    ListType = "Whitelist",
                    DateAdded = DateTime.Now
                };

                _networkAdmin.AddAddressToList(entry);
                WhitelistedAddresses.Add(entry);

                
                NewWhitelistAddress = string.Empty;

                MessageBox.Show("Address added to whitelist.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding to whitelist: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddToBlacklist()
        {
            if (string.IsNullOrWhiteSpace(NewBlacklistAddress))
            {
                MessageBox.Show("Address cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var entry = new AddressListEntry
                {
                    Address = NewBlacklistAddress,
                    Type = "IP Address",
                    ListType = "Blacklist",
                    DateAdded = DateTime.Now
                };

                _networkAdmin.AddAddressToList(entry);
                BlacklistedAddresses.Add(entry);

              
                NewBlacklistAddress = string.Empty;

                MessageBox.Show("Address added to blacklist.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding to blacklist: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveFromWhitelist()
        {
            if (SelectedWhitelistedAddress == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to remove {SelectedWhitelistedAddress.Address} from the whitelist?",
                "Remove Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                   
                    _networkAdmin.RemoveAddressFromList(SelectedWhitelistedAddress);

                   
                    WhitelistedAddresses.Remove(SelectedWhitelistedAddress);

                    MessageBox.Show("Address removed from whitelist.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error removing from whitelist: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RemoveFromBlacklist()
        {
            if (SelectedBlacklistedAddress == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to remove {SelectedBlacklistedAddress.Address} from the blacklist?",
                "Remove Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                   
                    _networkAdmin.RemoveAddressFromList(SelectedBlacklistedAddress);

                  
                    BlacklistedAddresses.Remove(SelectedBlacklistedAddress);

                    MessageBox.Show("Address removed from blacklist.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error removing from blacklist: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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