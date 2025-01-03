using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using monitorizare_trafic.ViewModels.Base;
using monitorizare_trafic.Models;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
using monitorizare_trafic.Services;

namespace monitorizare_trafic.ViewModels
{
    public class NetworkAnalystViewModel : ViewModelBase
    {
        public User CurrentUser { get; set; }
        private readonly TrafficMonitor _context;
        private readonly NetworkAnalyst _networkAnalyst;
        // Collections for data display
        private ObservableCollection<Report> _reports;
        public ObservableCollection<Report> Reports
        {
            get => _reports;
            set
            {
                _reports = value;
                OnPropertyChanged(nameof(Reports));
            }
        }
        public ObservableCollection<NetworkData> PacketData { get; } = new ObservableCollection<NetworkData>();

        // Selected items
        private Report _selectedReport;
        public Report SelectedReport
        {
            get => _selectedReport;
            set
            {
                SetProperty(ref _selectedReport, value);
                LoadReportDetails();
            }
        }

        // Filters
        private DateTime _startDate = DateTime.Now.AddDays(-7);
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                SetProperty(ref _startDate, value);
                LoadData();
            }
        }

        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                SetProperty(ref _endDate, value);
                LoadData();
            }
        }

        // Commands
        public ICommand ExportDataCommand { get; }
        public ICommand AnalyzePacketsCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand RefreshDataCommand { get; }

        public NetworkAnalystViewModel()
        {
            _context = new TrafficMonitor();
            _networkAnalyst = new NetworkAnalyst();
            // Initialize commands
            ExportDataCommand = new RelayCommand(ExportData);
            AnalyzePacketsCommand = new RelayCommand(AnalyzePackets);
            GenerateReportCommand = new RelayCommand(GenerateReport);
            //RefreshDataCommand = new RelayCommand(async _ => await LoadData());

            // Initial data load
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Load Reports
                var reportList = _networkAnalyst.GetReports();
                Reports = new ObservableCollection<Report>(reportList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Reports: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadReportDetails()
        {
            if (SelectedReport == null) return;

            try
            {
                var relatedPackets = _context.GetNetworkData()
                    .Where(p => p.Timestamp.Date == SelectedReport.CreatedDate.Date)
                    .ToList();

                PacketData.Clear();
                foreach (var packet in relatedPackets)
                {
                    PacketData.Add(packet);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading report details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportData(object parameter)
        {
            // TODO: Implement export functionality for reports and packet data
            MessageBox.Show("Export functionality not implemented yet.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AnalyzePackets(object parameter)
        {
            if (PacketData.Count == 0) return;

            var analysis = new PacketAnalysis
            {
                TotalPackets = PacketData.Count,
                AverageSize = PacketData.Average(p => p.DataSize),
                TopSourceIPs = PacketData.GroupBy(p => p.SourceIP)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .ToDictionary(g => g.Key, g => g.Count()),
                TopDestinationIPs = PacketData.GroupBy(p => p.DestinationIP)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .ToDictionary(g => g.Key, g => g.Count()),
                //ProtocolDistribution = PacketData.GroupBy(p => p.Protocol)
                //    .ToDictionary(g => g.Key, g => g.Count())
            };

            MessageBox.Show($"Analysis Complete!\n" +
                          $"Total Packets: {analysis.TotalPackets}\n" +
                          $"Average Size: {analysis.AverageSize:F2} bytes\n" +
                          $"Top Source IP: {analysis.TopSourceIPs.FirstOrDefault().Key}",
                          "Packet Analysis Results",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }

        private void GenerateReport(object parameter)
        {
            // TODO: Implement report generation logic
            MessageBox.Show("Report generation not implemented yet.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class PacketAnalysis
    {
        public int TotalPackets { get; set; }
        public double AverageSize { get; set; }
        public Dictionary<string, int> TopSourceIPs { get; set; }
        public Dictionary<string, int> TopDestinationIPs { get; set; }
        public Dictionary<int, int> ProtocolDistribution { get; set; }
    }
}