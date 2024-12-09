using monitorizare_trafic.Models;
using monitorizare_trafic.Services;
using System.Collections.Generic;
using System.Windows;
using System.Threading;
using System.Linq;
using System;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

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

        private ChartValues<int> PacketTrendValues = new ChartValues<int>();
        private List<string> TimeLabels = new List<string>();
        public UserView()
        {
            InitializeComponent();
            TrafficChart.Series[0].Values = PacketTrendValues;
             TrafficChart.AxisX[0].Labels = TimeLabels;
            _trafficMonitor = new TrafficMonitor();
            _trafficMonitor.PacketCaptured += OnPacketCaptured; // Abonăm la eveniment
            _trafficAnalyzer = new TrafficAnalyzer();
            _trafficAnalyzer.AlertGenerated += OnAlertGenerated;

            _trafficAnalyzer.TrafficTrends.CollectionChanged += UpdateChart;

        }
        private void OnAlertGenerated(string alert)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AlertsDataGrid.Items.Add(new { AlertMessage = alert }); // Adaugă alerta în DataGrid
            });
        }
        private void UpdateChart(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (TrafficTrend trend in e.NewItems)
                {
                    PacketTrendValues.Add(trend.PacketCount);
                    TimeLabels.Add(trend.Timestamp.ToString("HH:mm:ss"));

                    // Elimină datele mai vechi de 10 puncte pentru performanță
                    if (PacketTrendValues.Count > 10)
                    {
                        PacketTrendValues.RemoveAt(0);
                        TimeLabels.RemoveAt(0);
                    }
                }

                // Actualizare grafic
                TrafficChart.AxisX[0].Labels = TimeLabels;
            });
        }
        private void RefreshTrafficData_Click(object sender, RoutedEventArgs e)
        {
            // Reîncarcă datele curente din TrafficTrends
            TrafficChart.Series[0].Values = new ChartValues<int>(_trafficAnalyzer.TrafficTrends.Select(t => t.PacketCount));
            TrafficChart.AxisX[0].Labels = _trafficAnalyzer.TrafficTrends.Select(t => t.Timestamp.ToString("HH:mm:ss")).ToList();
        }
        private void ClearTrafficData_Click(object sender, RoutedEventArgs e)
        {
            PacketTrendValues.Clear();
            TimeLabels.Clear();
            TrafficChart.AxisX[0].Labels.Clear();
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
