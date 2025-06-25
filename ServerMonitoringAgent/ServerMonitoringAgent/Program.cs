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
using System.Runtime.CompilerServices;
using ServerMonitoringAgent.Logging;
using ServerMonitoringAgent.Monitor;

namespace LinuxMonitor
{
    internal class Program
    {
        static readonly Dictionary<string, Func<ILogger, IMonitor>> ParamsDict = new Dictionary<string, Func<ILogger, IMonitor>>(StringComparer.OrdinalIgnoreCase)
        {
            ["[NETWORK]"] = logger => new NetworkMonitor(logger),
            ["[SYSTIME]"] = logger => new SystemTimeMonitor(logger),
            ["[SYNCTIME]"] = logger => new SyncTimeMonitor(logger),
            ["[FILESHARE]"] = logger => new FileShareMonitor(logger),
            ["[DOCKER]"] = logger => new DockerMonitor(logger),
            ["[SDUCON]"] = logger => new SduContainerMonitor(logger),
            ["[POSTGRESCON]"] = logger => new PostgresContainerMonitor(logger),
            ["[ETCDCON]"] = logger => new EtcdContainerMonitor(logger),
            ["[DDMWEBADMINCON]"] = logger => new DdmWebAdminContainerMonitor(logger),
            ["[DDMWEBCON]"] = logger => new DdmWebContainerMonitor(logger),
            ["[DDMWEBAPICON]"] = logger => new DdmWebApiContainerMonitor(logger)
        };

        static async Task Main(string[] args)
        {
            var logger = new ConsoleLogger();

            foreach (var arg in args) 
            {
                if(ParamsDict.TryGetValue(arg.ToUpper(), out var generalMonitor))
                {
                    await generalMonitor(logger).MonitorAsync();
                }
                else
                {
                    logger.Warn($"Unknown monitor parameter: {arg}");
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
