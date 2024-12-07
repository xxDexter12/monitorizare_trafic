using monitorizare_trafic.Models;
using monitorizare_trafic.Services;
using System.Collections.Generic;
using System.Windows;

namespace monitorizare_trafic.View
{
    public partial class UserView : Window
    {
        private TrafficMonitor _trafficMonitor;

        public UserView()
        {
            InitializeComponent();
            _trafficMonitor = new TrafficMonitor();
        }

        private void btnStartMonitoring_Click(object sender, RoutedEventArgs e)
        {
            _trafficMonitor.StartMonitoring();
            MessageBox.Show("Monitoring started!");

            // Actualizează DataGrid-ul cu datele colectate
            RefreshTrafficData();
        }

        private void btnStopMonitoring_Click(object sender, RoutedEventArgs e)
        {
            _trafficMonitor.StopMonitoring();
            MessageBox.Show("Monitoring stopped!");
        }

        private void RefreshTrafficData()
        {
            // Obține datele colectate
            List<NetworkData> networkData = _trafficMonitor.CollectTrafficData();

            // Actualizează DataGrid-ul cu noile date
            PacketDataGrid.ItemsSource = networkData;
        }
    }
}
