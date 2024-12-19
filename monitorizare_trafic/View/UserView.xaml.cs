using System.Windows;
using monitorizare_trafic.Models;
using monitorizare_trafic.ViewModels;

namespace monitorizare_trafic.View
{
    public partial class UserView : Window
    {
        private readonly UserViewModel _viewModel;

        public UserView(User user = null)
        {
            InitializeComponent();

            // Create and set the ViewModel
            _viewModel = new UserViewModel();
            _viewModel.CurrentUser = user;
            DataContext = _viewModel;
        }
    }
}