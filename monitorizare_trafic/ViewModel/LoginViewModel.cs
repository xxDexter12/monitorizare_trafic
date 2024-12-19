using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using monitorizare_trafic.Models;
using monitorizare_trafic.Utils;
using monitorizare_trafic.View;
using monitorizare_trafic.ViewModels.Base;

namespace monitorizare_trafic.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private readonly Manager _manager;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand MinimizeCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel()
        {
            _manager = new Manager();
            LoginCommand = new RelayCommand(ExecuteLogin);
            MinimizeCommand = new RelayCommand(ExecuteMinimize);
            CloseCommand = new RelayCommand(ExecuteClose);
            RegisterCommand = new RelayCommand(ExecuteRegister);
        }

        private void ExecuteLogin()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            string passwordHash = SecurityHelper.ComputeHash(Password);

            using (var db = _manager.GetDataContext())
            {
                var user = db.GetTable<User>().FirstOrDefault(u => u.Username == Username && u.Password == passwordHash);

                if (user != null)
                {
                    MessageBox.Show($"Logged in as {user.Role}.");
                    OpenAppropriateWindow(user.Role);
                }
                else
                {
                    MessageBox.Show("Invalid credentials.");
                }
            }
        }

        private void OpenAppropriateWindow(string role)
        {
            Window window = null;

            switch (role)
            {
                case "Admin":
                    window = new AdministratorView();
                    break;
                case "Analyst":
                    //window = new AnalystView();
                    break;
                case "User":
                    window = new UserView();
                    break;
                default:
                    MessageBox.Show("Unknown role.");
                    return;
            }

            if (window != null)
            {
                window.Show();
                Application.Current.Windows[0].Close();
            }
        }

        private void ExecuteMinimize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ExecuteClose()
        {
            Application.Current.Shutdown();
        }

        private void ExecuteRegister()
        {
            var registerWindow = new RegisterView();
            registerWindow.Show();
            Application.Current.Windows[0].Close();
        }
    }
}