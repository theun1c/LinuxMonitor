using ServerMonitoringAgent.Logging.ConsoleLogging;
using ServerMonitoringAgent.Monitor.CPU;
using ServerMonitoringAgent.Monitor.DockerProcess;
using ServerMonitoringAgent.Monitor.DockerProcess.Containers.DDM;
using ServerMonitoringAgent.Monitor.DockerProcess.Containers.SDU;
using ServerMonitoringAgent.Monitor.DockerProcess.Containers.Etcd;
using ServerMonitoringAgent.Monitor.DockerProcess.Containers.Postgres;
using ServerMonitoringAgent.Monitor.DockerProcess.Containers;
using ServerMonitoringAgent.Monitor.Memory;
using ServerMonitoringAgent.Monitor.Network;
using ServerMonitoringAgent.Monitor.Storage;
using ServerMonitoringAgent.Monitor.SystemTime;

namespace LinuxMonitor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var logger = new ConsoleLogger();

            if (args.Length >= 1)
            {
                foreach (string arg in args)
                {
                    if (arg == "[NETWORK]")
                    {
                        await new NetworkMonitor(logger).MonitorAsync();
                    }
                    else if (arg == "[SYSTIME]")
                    {
                        await new SystemTimeMonitor(logger).MonitorAsync();
                    }
                    else if (arg == "[SYNCTIME]")
                    {
                        await new SyncTimeMonitor(logger).MonitorAsync();
                    }
                    else if (arg == "[FILESHARE]")
                    {
                        await new FileShareMonitor(logger).MonitorAsync();
                    }
                    else if (arg == "[DOCKER]")
                    {
                        await new DockerMonitor(logger).MonitorAsync();
                    }
                    else if (arg == "[SDUCON]")
                    {
                        await new SduContainerMonitor(logger).MonitorAsync();
                    }
                    else if (arg == "[POSTGRESCON]")
                    {
                        await new PostgresContainerMonitor(logger).MonitorAsync();
                    }
                    else if (arg == "[ETCDCON]")
                    {
                        await new EtcdContainerMonitor(logger).MonitorAsync();
                    }
                    else if (arg == "[DDMWEBADMINCON]")
                    {
                        await new DdmWebAdminContainerMonitor(logger).MonitorAsync();
                    }
                    else if(arg == "[DDMWEBCON]")
                    {
                        await new DdmWebContainerMonitor(logger).MonitorAsync();
                    }
                    else if (arg == "[DDMWEBAPICON]")
                    {
                        await new DdmWebApiContainerMonitor(logger).MonitorAsync();
                    }
                }
            }

            await Task.WhenAll(
                new CpuMonitor(logger).MonitorAsync(),
                new StorageMonitor(logger).MonitorAsync(),
                new MemoryMonitor(logger).MonitorAsync()
            );
        }
    }
}
