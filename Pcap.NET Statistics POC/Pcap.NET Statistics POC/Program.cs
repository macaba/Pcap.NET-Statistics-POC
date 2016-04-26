using PcapDotNet.Core;
using System;

namespace Pcap.NET_Statistics_POC
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = new NetMon();
            n.Start();
        }  
    }
}
