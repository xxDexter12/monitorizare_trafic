using System.Windows;
using monitorizare_trafic.Models;
using monitorizare_trafic.ViewModels;

namespace monitorizare_trafic.View
{
    /// <summary>
    /// Interaction logic for NetworkAnalystView.xaml
    /// </summary>
    public partial class NetworkAnalystView : Window
    {
        private readonly NetworkAnalystViewModel _viewModel;
        public NetworkAnalystView(User user=null)
        {
            InitializeComponent();

            _viewModel = new NetworkAnalystViewModel();
            _viewModel.CurrentUser = user;
            DataContext = _viewModel;
            // Note: DataContext is already set in XAML, but you could also set it here if preferred
            // DataContext = new NetworkAnalystViewModel();
        }
    }
}