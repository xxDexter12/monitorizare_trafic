using monitorizare_trafic.Models;
using monitorizare_trafic.Services;
using System.Collections.Generic;
using System.Windows;
using System.Threading;
using System.Linq;
using System;
using System.Windows.Controls;

namespace monitorizare_trafic.View
{
    public partial class UserView : Window
    {
        private TrafficMonitor _trafficMonitor;
        private bool is_monitoring = false;
        private List<NetworkData> _networkData = new List<NetworkData>();
        private int _currentPage = 1;
        private const int ItemsPerPage = 50;
        private TrafficAnalyzer _trafficAnalyzer;

        public UserView()
        {
            InitializeComponent();
            _trafficMonitor = new TrafficMonitor();
            _trafficMonitor.PacketCaptured += OnPacketCaptured; // Abonăm la eveniment
            _trafficAnalyzer = new TrafficAnalyzer();
            _trafficAnalyzer.AlertGenerated += OnAlertGenerated;
        }
        private void OnAlertGenerated(string alert)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AlertsDataGrid.Items.Add(new { AlertMessage = alert }); // Adaugă alerta în DataGrid
            });
        }

        private void btnStartMonitoring_Click(object sender, RoutedEventArgs e)
        {
            _trafficMonitor.StartMonitoring();
            is_monitoring = true;
            MessageBox.Show("Monitoring started!");

            StartContinuousRefresh();
        }

        private void btnStopMonitoring_Click(object sender, RoutedEventArgs e)
        {
            _trafficMonitor.StopMonitoring();
            is_monitoring = false;
            MessageBox.Show("Monitoring stopped!");
        }

        private void StartContinuousRefresh()
        {
            // Folosim un timer pentru a actualiza mai eficient
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500); // Actualizare la fiecare 500ms
            timer.Tick += (s, e) =>
            {
                if (is_monitoring)
                {
                    // Dacă monitorizarea este activă, actualizăm DataGrid-ul
                    UpdatePagedData();
                }
            };
            timer.Start();
        }

        private void OnPacketCaptured(NetworkData packetData)
        {
            // Adăugăm pachetul în lista de date
            Application.Current.Dispatcher.Invoke(() =>
            {
                _networkData.Add(packetData); // Adăugăm pachetul în lista de date
                UpdatePagedData(); // Actualizăm DataGrid-ul
            });
        }

        private void UpdatePagedData()
        {
            var pagedData = _networkData.Skip((_currentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
            PacketDataGrid.ItemsSource = pagedData;

            TotalPagesTextBlock.Text = $"Page {_currentPage} of {(_networkData.Count / ItemsPerPage) + 1}";
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage * ItemsPerPage < _networkData.Count)
            {
                _currentPage++;
                UpdatePagedData(); // Actualizăm DataGrid-ul
            }
        }

        private void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                UpdatePagedData(); // Actualizăm DataGrid-ul
            }
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.TabControl.Visibility = Visibility.Collapsed;
            LiveTrafficPanel.Visibility = Visibility.Collapsed;
            TrafficTrendsPanel.Visibility = Visibility.Collapsed;
            AlertsPanel.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TabControl.Visibility = Visibility.Visible;
            LiveTrafficPanel.Visibility = Visibility.Visible;
            TrafficTrendsPanel.Visibility = Visibility.Visible;
            AlertsPanel.Visibility = Visibility.Collapsed;
        }
    }
}
