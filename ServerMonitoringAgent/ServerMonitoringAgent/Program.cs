using ServerMonitoringAgent.Logging.ConsoleLogging;
using ServerMonitoringAgent.Monitor.CPU;
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
