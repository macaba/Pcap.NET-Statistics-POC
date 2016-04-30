using Topshelf;

namespace Pcap.NET_Statistics_POC
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<NetMon>(s =>
                {
                    s.ConstructUsing(name => new NetMon());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("pCapNetStats");
                x.SetDisplayName("Pcap.NET Statistics POC");
                x.SetServiceName("pcapstats");
                x.StartAutomaticallyDelayed();
            });
        }  
    }
}
