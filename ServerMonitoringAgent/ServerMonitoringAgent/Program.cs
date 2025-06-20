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

            // ПОЛУЧАЮ СПИСКОМ МЕТКИ [] из аргументов консоли - чета типо фильтров
            // TODO: сделать выполнение команд исходя из полученных "фильтров" аргументов консоли
            // TODO: сделать доку + сделать удобный вывод [CPU] 10% вместо каких-то строк...
            var logger = new ConsoleLogger();
            await Task.WhenAll(
                new NetworkMonitor(logger).MonitorAsync(),
                new SyncTimeMonitor(logger).MonitorAsync(),
                new SystemTimeMonitor(logger).MonitorAsync(),
                new CpuMonitor(logger).MonitorAsync(),
                new StorageMonitor(logger).MonitorAsync(),
                new MemoryMonitor(logger).MonitorAsync()
            );
        }
    }
}
