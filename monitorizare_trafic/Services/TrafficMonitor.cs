using monitorizare_trafic.Models;
using System;
using SharpPcap;
using PacketDotNet;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitorizare_trafic.Services
{
    public class TrafficMonitor
    {
        private ICaptureDevice _device;
        public List<NetworkData> CollectTrafficData()
        {
            // Codul pentru colectarea datelor
            return new List<NetworkData>();
        }

        public void StartMonitoring()
        {
            var devices = CaptureDeviceList.Instance;

            if(devices.Count<1)
            {
                Console.WriteLine("No devices found.");
                return;
            }

            _device = devices[0];
            Console.WriteLine($"Using device: {_device.Description}");

            _device.OnPacketArrival += OnPacketArrival;
            _device.Open(DeviceModes.Promiscuous);

            Console.WriteLine("Starting capture...");
            _device.StartCapture();
        }

        private void OnPacketArrival(object sender, PacketCapture e)
        {
            try
            {
                var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);

                var ipPacket = packet.Extract<IPPacket>();
                if (ipPacket!=null)
                {
                    string sourceIP = ipPacket.SourceAddress.ToString();
                    string destIP = ipPacket.DestinationAddress.ToString();
                    int packetSize = ipPacket.PayloadLength;

                    Console.WriteLine($"Packet captured: {sourceIP} -> {destIP}, Size: {packetSize} bytes");
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


    }
}
