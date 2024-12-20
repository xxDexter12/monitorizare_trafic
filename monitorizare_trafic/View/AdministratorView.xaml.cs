using System.Windows;
using System.Windows.Input;

namespace monitorizare_trafic.View
{
    public partial class AdministratorView : Window
    {
        public AdministratorView()
        {
            InitializeComponent();
        }

        // Only window-specific functionality remains in code-behind
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}