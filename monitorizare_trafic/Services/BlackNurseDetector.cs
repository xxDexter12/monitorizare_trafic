using System;
using SharpPcap;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketDotNet;

namespace monitorizare_trafic.Services
{
    public class BlackNurseDetector
    {
        private const int DetectionThreshold = 10;
        private const int TimeWindowInSeconds = 10;

        private Dictionary<string, List<DateTime>> _icmplogs = new Dictionary<string, List<DateTime>>();

        public void AddIcmpPacket(string sourceIp, IcmpV4Packet packet)
        {   

            if(packet.TypeCode==IcmpV4TypeCode.UnreachablePort)
            {
                if (!_icmplogs.ContainsKey(sourceIp))
                {
                    _icmplogs[sourceIp]=new List<DateTime>();
                }

                _icmplogs[sourceIp].Add(DateTime.Now);
                _icmplogs[sourceIp] = _icmplogs[sourceIp]
                    .Where(timestamp => timestamp > DateTime.Now.AddSeconds(-TimeWindowInSeconds))
                    .ToList();

                if (_icmplogs[sourceIp].Count>=DetectionThreshold)
                {
                    Console.WriteLine($"BlackNurse attack detected from {sourceIp}!");
                    //TODO ADAUGA ALERTA

                }

            }
        }
    }
}
