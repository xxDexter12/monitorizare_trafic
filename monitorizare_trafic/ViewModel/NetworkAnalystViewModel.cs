using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using monitorizare_trafic.ViewModels.Base;
using monitorizare_trafic.Models;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;
using monitorizare_trafic.Services;
using System.Text;
using System.Data.Linq;
using monitorizare_trafic.Utils;


namespace monitorizare_trafic.ViewModels
{
    public class NetworkAnalystViewModel : ViewModelBase
    {
        public User CurrentUser { get; set; }
        private readonly TrafficMonitor _context;
        private readonly NetworkAnalyst _networkAnalyst;
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

                public ICommand ExportDataCommand { get; }
        public ICommand AnalyzePacketsCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand RefreshDataCommand { get; }
        private readonly Manager _manager;
        public NetworkAnalystViewModel()
        {
            _context = new TrafficMonitor();
            _manager = new Manager();
            _networkAnalyst = new NetworkAnalyst();
            SuspiciousPackets = new ObservableCollection<NetworkData>();
                        ExportDataCommand = new RelayCommand(ExportData);
            AnalyzePacketsCommand = new RelayCommand(AnalyzePackets);
            GenerateReportCommand = new RelayCommand(GenerateReport);
            
            _selectedPackets = new List<NetworkData>();
            AddToSuspiciousCommand = new RelayCommand(AddToSuspicious, CanAddToSuspicious);
            _context = new TrafficMonitor();
            _networkAnalyst = new NetworkAnalyst();
            SuspiciousPackets = new ObservableCollection<NetworkData>();

                        LoadData();
        }


        public void UpdateSelectedPackets(List<NetworkData> selectedItems)
        {
            SelectedPackets = selectedItems ?? new List<NetworkData>();
        }

        private bool CanAddToSuspicious(object parameter)
        {
            return SelectedPackets != null && SelectedPackets.Any();
        }

        private void AddToSuspicious(object parameter)
        {
            if (SelectedPackets == null || !SelectedPackets.Any()) return;

            foreach (var packet in SelectedPackets)
            {
                if (!SuspiciousPackets.Contains(packet))
                {
                    SuspiciousPackets.Add(packet);
                }
            }
        }

        private void LoadData()
        {
            try
            {
                                var reportList = _networkAnalyst.GetReports()
                    .Where(report => HasPacketsForReport(report))
                    .ToList();
                Reports = new ObservableCollection<Report>(reportList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Reports: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool HasPacketsForReport(Report report)
        {
            return _context.GetNetworkData()
                .Any(p => p.Timestamp.Date == report.CreatedDate.Date);
        }
        public ICommand AddToSuspiciousCommand { get; }
        private List<NetworkData> _selectedPackets;
        public List<NetworkData> SelectedPackets
        {
            get => _selectedPackets;
            private set
            {
                _selectedPackets = value;
                OnPropertyChanged(nameof(SelectedPackets));
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
                                            };

            MessageBox.Show($"Analysis Complete!\n" +
                          $"Total Packets: {analysis.TotalPackets}\n" +
                          $"Average Size: {analysis.AverageSize:F2} bytes\n" +
                          $"Top Source IP: {analysis.TopSourceIPs.FirstOrDefault().Key}",
                          "Packet Analysis Results",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }
        private ObservableCollection<NetworkData> _suspiciousPackets;
        public ObservableCollection<NetworkData> SuspiciousPackets
        {
            get => _suspiciousPackets;
            set
            {
                _suspiciousPackets = value;
                OnPropertyChanged(nameof(SuspiciousPackets));
            }
        }

        private void GenerateReport(object parameter)
        {
            if (SelectedReport == null)
            {
                MessageBox.Show("Selectați un raport pentru a continua.",
                    "Avertizare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                                var suspiciousPacketsList = SuspiciousPackets.ToList();

                                var eventReport = new EventReport
                {
                    ReportId = SelectedReport.ReportId,
                    AnalystComments = GenerateAnalysisContent(PacketData.ToList(), suspiciousPacketsList),
                                        SuspiciousPackets = string.Join(",", suspiciousPacketsList.Select(p =>
                        $"{p.SourceIP}->{p.DestinationIP}:{p.Port}:{p.DataSize}")),
                    CreatedDate = DateTime.Now,
                    AnalystId = CurrentUser?.UserId ?? 0
                };

                                _manager.AddEventReport(eventReport);
                _manager.UpdateReportStatus(SelectedReport.ReportId, "Resolved");

                                LoadData();

                MessageBox.Show("Raport generat și salvat cu succes!",
                    "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la salvarea raportului: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private List<NetworkData> AnalyzeSuspiciousPackets(List<NetworkData> packets)
        {
            var suspicious = new List<NetworkData>();

            foreach (var packet in packets)
            {
                if (IsSuspiciousPort(packet.Port) ||
                    IsUnusualDataSize(packet.DataSize))
                {
                    suspicious.Add(packet);
                }
            }

            return suspicious;
        }

        private string GenerateAnalysisContent(List<NetworkData> allPackets, List<NetworkData> suspiciousPackets)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Analiză trafic pentru raportul: {SelectedReport.Title}");
            sb.AppendLine($"Data analizei: {DateTime.Now}");
            sb.AppendLine($"Total pachete analizate: {allPackets.Count}");
            sb.AppendLine($"Pachete suspecte detectate: {suspiciousPackets.Count}");

            if (suspiciousPackets.Any())
            {
                sb.AppendLine("\nDetalii pachete suspecte:");
                foreach (var packet in suspiciousPackets)
                {
                    sb.AppendLine($"- Pachet suspect de la {packet.SourceIP} către {packet.DestinationIP}");
                    sb.AppendLine($"  Port: {packet.Port}, Mărime: {packet.DataSize} bytes");
                    sb.AppendLine($"  Timestamp: {packet.Timestamp}");
                }
            }

            return sb.ToString();
        }

        private bool IsSuspiciousPort(int port)
        {
            int[] suspiciousPorts = { 31337, 12345, 27374, 31338, 31339 };
            return suspiciousPorts.Contains(port);
        }

        private bool IsUnusualDataSize(int size)
        {
            return size > 65000 || size == 1234;
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