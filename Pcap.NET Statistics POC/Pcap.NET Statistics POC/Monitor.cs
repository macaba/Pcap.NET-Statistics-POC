using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PcapDotNet.Core;

namespace Pcap.NET_Statistics_POC
{
    class Monitor
    {
        private LivePacketDevice _selectedDevice = null;
        private string _name;

        public Monitor(string name)
        {
            _name = name;
        }

        public bool Connect(string subnet)
        {
            IList<LivePacketDevice> devices = LivePacketDevice.AllLocalMachine;

            foreach (var device in devices)
            {
                if (device.Addresses.Any(a => a.Address.ToString().StartsWith($"Internet {subnet}")))
                {
                    _selectedDevice = device;
                    break;
                }
            }

            if (_selectedDevice == null)
            {
                return false;
            }

            return true;
        }

        public async void GetStats(string filter, CancellationTokenSource cnx)
        {
            PacketCommunicator comm = _selectedDevice.Open(100, PacketDeviceOpenAttributes.None, 1000);
            comm.Mode = PacketCommunicatorMode.Statistics;
            comm.SetFilter(filter);
            await Task.Run(() =>
            {            
                comm.ReceiveStatistics(0, ReceiveStatisticsHandler);
            }, cnx.Token);
        }

        private DateTime _lastTimestamp;

        private void ReceiveStatisticsHandler(PacketSampleStatistics statistics)
        {
            DateTime currentTimestamp = statistics.Timestamp;
            DateTime previousTimestamp = _lastTimestamp;
            _lastTimestamp = currentTimestamp;

            if (previousTimestamp == DateTime.MinValue)
                return;

            double delayInSeconds = (currentTimestamp - previousTimestamp).TotalSeconds;
            double bitsPerSecond = statistics.AcceptedBytes * 8 / delayInSeconds;
            double packetsPerSecond = statistics.AcceptedPackets / delayInSeconds;

            Console.WriteLine($"{statistics.Timestamp} {_name} BPS: {bitsPerSecond} PPS: {packetsPerSecond}");
        }


    }
}
