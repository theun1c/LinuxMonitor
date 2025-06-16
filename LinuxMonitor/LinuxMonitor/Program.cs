using LinuxMonitor.BashExecutor;
using LinuxMonitor.Logging;
using LinuxMonitor.Monitor;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

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
