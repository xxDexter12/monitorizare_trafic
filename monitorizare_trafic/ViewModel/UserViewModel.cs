using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using monitorizare_trafic.ViewModels.Base;
using monitorizare_trafic.Models;
using monitorizare_trafic.Services;
using System.Windows;
using LiveCharts;
using System.Linq;
using System.Collections.Generic;
using monitorizare_trafic.Utils;

namespace monitorizare_trafic.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private readonly TrafficMonitor _trafficMonitor;
        //private readonly TrafficAnalyzer _trafficAnalyzer;
        private bool _isMonitoring;
        private int _currentPage = 1;
        private const int ItemsPerPage = 50;

        public User CurrentUser { get; set; }

        // Observable Collections for UI
        public ObservableCollection<NetworkData> NetworkData { get; } = new ObservableCollection<NetworkData>();
        public ObservableCollection<AlertInfo> Alerts { get; } = new ObservableCollection<AlertInfo>();

        // Chart Properties
        public ChartValues<int> PacketTrendValues { get; } = new ChartValues<int>();
        public List<string> TimeLabels { get; } = new List<string>();

        // UI Visibility Properties
        private Visibility _alertsPanelVisibility = Visibility.Collapsed;
        public Visibility AlertsPanelVisibility
        {
            get => _alertsPanelVisibility;
            set => SetProperty(ref _alertsPanelVisibility, value);
        }

        private Visibility _mainContentVisibility = Visibility.Visible;
        public Visibility MainContentVisibility
        {
            get => _mainContentVisibility;
            set => SetProperty(ref _mainContentVisibility, value);
        }

        // Report Properties
        private string _reportName;
        public string ReportName
        {
            get => _reportName;
            set => SetProperty(ref _reportName, value);
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        private int _selectedPriority = 1;
        public int SelectedPriority
        {
            get => _selectedPriority;
            set => SetProperty(ref _selectedPriority, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        // Commands
        public ICommand StartMonitoringCommand { get; }
        public ICommand StopMonitoringCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand ShowAlertsCommand { get; }
        public ICommand ShowDashboardCommand { get; }
        public ICommand RefreshDataCommand { get; }
        public ICommand ClearDataCommand { get; }
        public ICommand SubmitReportCommand { get; }
        public ICommand MinimizeCommand { get; }
        public ICommand CloseCommand { get; }

        public UserViewModel()
        {
            _trafficMonitor = new TrafficMonitor();
            _trafficMonitor.Analyzer = new TrafficAnalyzer();
            // Initialize Commands
            StartMonitoringCommand = new RelayCommand(_ => StartMonitoring());
            StopMonitoringCommand = new RelayCommand(_ => StopMonitoring());
            NextPageCommand = new RelayCommand(_ => NextPage(), _ => CanNavigateNext());
            PreviousPageCommand = new RelayCommand(_ => PreviousPage(), _ => CanNavigatePrevious());
            ShowAlertsCommand = new RelayCommand(_ => ShowAlerts());
            ShowDashboardCommand = new RelayCommand(_ => ShowDashboard());
            RefreshDataCommand = new RelayCommand(_ => RefreshData());
            ClearDataCommand = new RelayCommand(_ => ClearData());
            SubmitReportCommand = new RelayCommand(_ => SubmitReport());
            MinimizeCommand = new RelayCommand(_ => Application.Current.MainWindow.WindowState = WindowState.Minimized);
            CloseCommand = new RelayCommand(_ => Application.Current.Shutdown());

            // Setup Event Handlers
            _trafficMonitor.PacketCaptured += OnPacketCaptured;
            _trafficMonitor.Analyzer.AlertGenerated += OnAlertGenerated;
            _trafficMonitor.Analyzer.TrafficTrends.CollectionChanged += UpdateChart;
        }

        private void StartMonitoring()
        {
            _trafficMonitor.StartMonitoring();
            _isMonitoring = true;
            MessageBox.Show("Monitoring started!");
            StartContinuousRefresh();
        }

        private void StopMonitoring()
        {
            _trafficMonitor.StopMonitoring();
            _isMonitoring = false;
            MessageBox.Show("Monitoring stopped!");
        }

        private void OnPacketCaptured(NetworkData packetData)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                NetworkData.Add(packetData);
                UpdatePagedData();
            });
        }

        private void OnAlertGenerated(string alert)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Alerts.Add(new AlertInfo { AlertMessage = alert });
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

                    if (PacketTrendValues.Count > 10)
                    {
                        PacketTrendValues.RemoveAt(0);
                        TimeLabels.RemoveAt(0);
                    }
                }
            });
        }

        private void ShowAlerts()
        {
            AlertsPanelVisibility = Visibility.Visible;
            MainContentVisibility = Visibility.Collapsed;
        }

        private void ShowDashboard()
        {
            AlertsPanelVisibility = Visibility.Collapsed;
            MainContentVisibility = Visibility.Visible;
        }

        private void RefreshData()
        {
            PacketTrendValues.Clear();
            var trends = _trafficMonitor.Analyzer.TrafficTrends.ToList();
            foreach (var trend in trends)
            {
                PacketTrendValues.Add(trend.PacketCount);
                TimeLabels.Add(trend.Timestamp.ToString("HH:mm:ss"));
            }
        }

        private void ClearData()
        {
            PacketTrendValues.Clear();
            TimeLabels.Clear();
        }

        private void SubmitReport()
        {
            if (string.IsNullOrWhiteSpace(ReportName) || string.IsNullOrWhiteSpace(SelectedCategory))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var report = new Report
            {
                Title = ReportName,
                Category = SelectedCategory,
                Priority = SelectedPriority,
                Description = Description,
                CreatedDate = DateTime.Now,
                CreatedBy = CurrentUser.UserId
            };

            try
            {
                Manager manager = new Manager();
                var db = manager.GetDataContext();
                db.GetTable<Report>().InsertOnSubmit(report);
                db.SubmitChanges();

                MessageBox.Show("Report submitted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearReportFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearReportFields()
        {
            ReportName = string.Empty;
            SelectedCategory = null;
            SelectedPriority = 1;
            Description = string.Empty;
        }

        private void UpdatePagedData()
        {
            var pagedData = NetworkData.Skip((_currentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
            // Update UI bound collection here
        }

        private void NextPage()
        {
            if (CanNavigateNext())
            {
                _currentPage++;
                UpdatePagedData();
            }
        }

        private void PreviousPage()
        {
            if (CanNavigatePrevious())
            {
                _currentPage--;
                UpdatePagedData();
            }
        }

        private bool CanNavigateNext() => _currentPage * ItemsPerPage < NetworkData.Count;
        private bool CanNavigatePrevious() => _currentPage > 1;

        private void StartContinuousRefresh()
        {
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            timer.Tick += (s, e) =>
            {
                if (_isMonitoring)
                {
                    UpdatePagedData();
                }
            };
            timer.Start();
        }
    }

    public class AlertInfo
    {
        public string AlertMessage { get; set; }
    }
}