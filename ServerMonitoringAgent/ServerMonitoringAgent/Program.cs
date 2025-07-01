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
using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Executors;

namespace LinuxMonitor
{
    internal class Program
    {
        static readonly Dictionary<string, Func<ILogger, ILinuxExecutor, IMonitor>> ParamsDict = new Dictionary<string, Func<ILogger, ILinuxExecutor, IMonitor>>(StringComparer.OrdinalIgnoreCase)
        {
            ["[NETWORK]"] = (logger, executor) => new NetworkMonitor(logger, executor),
            ["[SYSTIME]"] = (logger, executor) => new SystemTimeMonitor(logger, executor),
            ["[SYNCTIME]"] = (logger, executor) => new SyncTimeMonitor(logger, executor),
            ["[FILESHARE]"] = (logger, executor) => new FileShareMonitor(logger, executor),
            ["[DOCKER]"] = (logger, executor) => new DockerMonitor(logger, executor),
            ["[SDUCON]"] = (logger, executor) => new SduContainerMonitor(logger, executor),
            ["[POSTGRESCON]"] = (logger, executor) => new PostgresContainerMonitor(logger, executor),
            ["[ETCDCON]"] = (logger, executor) => new EtcdContainerMonitor(logger, executor),
            ["[DDMWEBADMINCON]"] = (logger, executor) => new DdmWebAdminContainerMonitor(logger, executor),
            ["[DDMWEBCON]"] = (logger, executor) => new DdmWebContainerMonitor(logger, executor),
            ["[DDMWEBAPICON]"] = (logger, executor) => new DdmWebApiContainerMonitor(logger, executor)
        };

        static async Task Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var executor = new LinuxExecutor(logger);

            foreach (var arg in args) 
            {
                if(ParamsDict.TryGetValue(arg.ToUpper(), out var generalMonitor))
                {
                    await generalMonitor(logger, executor).MonitorAsync();
                }
                else
                {
                    logger.Warn($"Unknown monitor parameter: {arg}");
                }
            }

            await Task.WhenAll(
                new CpuMonitor(logger, executor).MonitorAsync(),
                new StorageMonitor(logger, executor).MonitorAsync(),
                new MemoryMonitor(logger, executor).MonitorAsync()
            );
        }
    }
}
