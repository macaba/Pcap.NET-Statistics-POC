using System;
using System.Threading;

namespace Pcap.NET_Statistics_POC
{
    public class NetMon
    {
        public void Start()
        {
            var cnx = new CancellationTokenSource();

            var localMonitor = new Monitor("LOC");
            if(localMonitor.Connect(Properties.Settings.Default.LocalAdapter))
                localMonitor.GetStats($"dst net {Properties.Settings.Default.LocalNet}", cnx);

            var remoteMonitor = new Monitor("REM");
            if(remoteMonitor.Connect(Properties.Settings.Default.LocalAdapter))
                remoteMonitor.GetStats($"not dst net {Properties.Settings.Default.LocalNet}", cnx);

            Console.ReadKey();
            cnx.Cancel();
        }
    }
}
