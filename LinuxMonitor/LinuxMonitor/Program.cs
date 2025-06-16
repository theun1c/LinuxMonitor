using LinuxMonitor.Logging.ConsoleLogging;
using LinuxMonitor.Monitor.CPU;
using LinuxMonitor.Monitor.Memory;
using LinuxMonitor.Monitor.Storage;

namespace LinuxMonitor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var logger = new ConsoleLogger(); 

            await Task.WhenAll(
                new CpuMonitor(logger).MonitorAsync(),
                new StorageMonitor(logger).MonitorAsync(),
                new MemoryMonitor(logger).MonitorAsync()
            );
        }
    }
}
