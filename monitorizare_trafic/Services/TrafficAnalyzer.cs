using System;
using System.Collections.Generic;
using monitorizare_trafic.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace monitorizare_trafic.Services
{
    public class TrafficAnalyzer
    {
        private Dictionary<string, List<ScanAttempt>> _scanAttempts = new Dictionary<string, List<ScanAttempt>>();
        private const int ScanThreshold = 100;
        private const int TimeWindowInSeconds = 10;
        private const int ScanPortThreshold = 5;
        public ObservableCollection<string> Alerts { get; private set; } = new ObservableCollection<string>();
        public event Action<string> AlertGenerated;

        private ObservableCollection<TrafficTrend> _trafficTrends = new ObservableCollection<TrafficTrend>();
        public ObservableCollection<TrafficTrend> TrafficTrends => _trafficTrends;
        public void UpdateTrafficTrends(int packetCount)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                _trafficTrends.Add(new TrafficTrend
                {
                    Timestamp = DateTime.Now,
                    PacketCount = packetCount
                });

                // Păstrăm doar ultimele 10 minute de date pentru trend
                while (_trafficTrends.Count > 0 &&
                       _trafficTrends[0].Timestamp < DateTime.Now.AddMinutes(-10))
                {
                    _trafficTrends.RemoveAt(0);
                }
            });
        }
        private class ScanAttempt
        {
            public string sourceIp { get; set; }
            public string targetIp { get; set; }
            public int destPort { get; set; }
            public DateTime Timestamp { get; set; }
        }
        public void AddScanAttempt(string srcIP, string destIP, int destPort)
        {
            var scanAttempt = new ScanAttempt
            {
                sourceIp = srcIP,
                targetIp = destIP,
                destPort = destPort,
                Timestamp = DateTime.Now
            };

            if (!_scanAttempts.ContainsKey(srcIP))
            {
                _scanAttempts[srcIP] = new List<ScanAttempt>();
            }
            _scanAttempts[srcIP].Add(scanAttempt);

            //_scanAttempts[srcIP] = _scanAttempts[srcIP].Where(a => a.Timestamp > DateTime.Now.AddSeconds(-TimeWindowInSeconds)).ToList();

            DetectScan(srcIP, destIP);

        }

        private void DetectScan(string srcIP, string destIP)
        {
            var recentAttempts = _scanAttempts[srcIP]
                .Where(a => a.targetIp == destIP)
                .GroupBy(a => a.destPort)
                .Where(g => g.Count() >= ScanPortThreshold)
                .ToList();
            if (recentAttempts.Count() >= ScanPortThreshold)
            {
                var alert = $"Potential host scan detected from {srcIP} to {destIP}. Scan type: Port scan.";

                // Adăugăm alerta în colecția ObservableCollection
                App.Current.Dispatcher.Invoke(() =>
                {
                    Alerts.Add(alert); // Modificăm ObservableCollection
                });

                AlertGenerated.Invoke(alert);
            }
        }
    }
}
