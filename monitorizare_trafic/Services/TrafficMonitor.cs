using monitorizare_trafic.Models;
using System;
using SharpPcap;
using PacketDotNet;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;
using monitorizare_trafic.Utils;

namespace monitorizare_trafic.Services
{
    public class TrafficMonitor
    {
        private ICaptureDevice _device;
        private Manager _manager = new Manager();
        public TrafficAnalyzer Analyzer = new TrafficAnalyzer();
        private BlackNurseDetector _blackNurseDetector = new BlackNurseDetector();
        public List<NetworkData> NetworkData = new List<NetworkData>();
        private int packetCount = 0;
        private ObservableCollection<NetworkData> trafficData = new ObservableCollection<NetworkData>();

        public void StartMonitoring()
        {
            var devices = CaptureDeviceList.Instance;

            if (devices.Count < 1)
            {
                Console.WriteLine("No devices found.");
                return;
            }

            for (int i = 0; i < devices.Count; i++)
            {
                Console.WriteLine($"Device number: {i}\n Device: {devices[i].Description}");

            }

            //momentan lasam device 10 ca ala merge la mine pe laptop, de adaugat la monitorizare
            //abilitatea sa schimbi device-ul
            _device = devices[10];
            Console.WriteLine($"Using device: {_device.Description}");

            _device.OnPacketArrival += OnPacketArrival;
            _device.Open(DeviceModes.Promiscuous);

            Console.WriteLine("Starting capture...");
            _device.StartCapture();
        }
        public event Action<NetworkData> PacketCaptured;
        private void OnPacketArrival(object sender, PacketCapture e)
        {
            //if(this.packetCount==10)
            //{
            //    StopMonitoring();
            //    return;
            //}
            //this.packetCount++;
            try
            {
                var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);

                var ipPacket = packet.Extract<IPPacket>();
                if (ipPacket != null)
                {
                    string sourceIP = ipPacket.SourceAddress.ToString();
                    string destIP = ipPacket.DestinationAddress.ToString();
                    int packetSize = ipPacket.PayloadLength;
                    packetCount++;

                    Analyzer.UpdateTrafficTrends(packetCount);
                    int destPort = -1;
                    if (ipPacket.Protocol == ProtocolType.Tcp)
                    {
                        destPort = packet.Extract<TcpPacket>().DestinationPort;
                    }
                    else if (ipPacket.Protocol == ProtocolType.Udp)
                    {
                        destPort = packet.Extract<UdpPacket>().DestinationPort;
                    }
                    else if (ipPacket.Protocol == ProtocolType.Icmp)
                    {
                        _blackNurseDetector.AddIcmpPacket(sourceIP, packet.Extract<IcmpV4Packet>());
                    }

                    if (destPort != -1)
                    {
                        Task.Run(() => Analyzer.AddScanAttempt(sourceIP, destIP, destPort));
                    }

                    // Adaugă un nou obiect în colecția ObservableCollection
                    NetworkData packetData = new NetworkData
                    {
                        Id = packetCount,
                        SourceIP = sourceIP,
                        DestinationIP = destIP,
                        Port = destPort,
                        DataSize = packetSize
                    };

                    _manager.AddNetworkData(packetData);

                    // Trimite evenimentul
                    PacketCaptured?.Invoke(packetData);

                    //Console.WriteLine($"Packet captured: {sourceIP} -> {destIP}, Size: {packetSize} bytes");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing packet: {ex.Message}");
            }
        }

        public void StopMonitoring()
        {

            _device?.StopCapture();
            _device?.Close();
            Console.WriteLine("Capture stopped.");
        }

        public List<NetworkData> GetNetworkData()
        {
            using (var context=_manager.GetDataContext())
            {
                return context.GetTable<NetworkData>().ToList();
            }
        }

    }
}
