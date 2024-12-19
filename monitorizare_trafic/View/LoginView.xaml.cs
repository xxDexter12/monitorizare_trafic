using System.Windows;
using System.Windows.Input;
using monitorizare_trafic.ViewModel;
using monitorizare_trafic.ViewModels;

namespace monitorizare_trafic.View
{
    public partial class LoginView : Window
    {
        private readonly LoginViewModel _viewModel;
        public LoginView()
        {
            InitializeComponent();
            _viewModel= new LoginViewModel();
            DataContext = _viewModel;
    }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}