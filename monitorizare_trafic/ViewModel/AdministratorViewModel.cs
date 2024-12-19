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
    public class AdministratorViewModel : ViewModelBase
    {
        private NetworkAdmin networkadmin;

        public AdministratorViewModel()
        {
            networkadmin = new NetworkAdmin();
        }


    }
}
