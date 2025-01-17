﻿using System.Windows;
using System.Windows.Controls;
using monitorizare_trafic.Models;
using monitorizare_trafic.ViewModels;
using System.Linq;
using System.Collections;

namespace monitorizare_trafic.View
{
    public partial class NetworkAnalystView : Window
    {
        private readonly NetworkAnalystViewModel _viewModel;
        public NetworkAnalystView(User user=null)
        {
            InitializeComponent();

            _viewModel = new NetworkAnalystViewModel();
            _viewModel.CurrentUser = user;
            DataContext = _viewModel;
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is NetworkAnalystViewModel viewModel)
            {
                var dataGrid = sender as DataGrid;
                if (dataGrid?.SelectedItems != null)
                {
                    var selectedItems = dataGrid.SelectedItems
                        .OfType<NetworkData>()
                        .ToList();
                    viewModel.UpdateSelectedPackets(selectedItems);
                }
            }
        }
    }
}