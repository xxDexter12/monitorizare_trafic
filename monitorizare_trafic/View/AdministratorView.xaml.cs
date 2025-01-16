using System.Windows;
using System.Windows.Input;
using monitorizare_trafic.Models;
using monitorizare_trafic.ViewModel;
using monitorizare_trafic.ViewModels;

namespace monitorizare_trafic.View
{
    public partial class AdministratorView : Window
    {
        private readonly AdministratorViewModel _viewModel;
        public AdministratorView(User user=null)
        {
            InitializeComponent();
            _viewModel = new AdministratorViewModel();
            _viewModel.CurrentUser = user;
            DataContext = _viewModel;
        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}