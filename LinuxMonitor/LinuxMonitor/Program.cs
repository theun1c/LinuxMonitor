
using System.Diagnostics;

namespace LinuxMonitor
{
    internal class Program
    {
        /// <summary>
        /// NOTE: тест работы потоков
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var cpuThread = new Thread(CPUMonitor);
            cpuThread.Start();
            var memoryThread = new Thread(MemoryMonitor);
            memoryThread.Start();
            var memoryInfo = GC.GetGCMemoryInfo();
            Console.WriteLine($"Доступно памяти: {memoryInfo.MemoryLoadBytes / 1024 / 1024} MB");
        }

        private static void CPUMonitor()
        {
            var CPUCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            while (true) 
            {
                // CPU usage
                Console.WriteLine($"CPU usage: {CPUCounter.NextValue()}%");
                Thread.Sleep(500);
            }
        }

        private static void MemoryMonitor()
        {
            var memoryCounter = new PerformanceCounter("Memory", "Available MBytes");

            int total = 16384; // GB
            while (true)
            {
                Console.WriteLine($"Memory usage: {100 - ((memoryCounter.NextValue() / total) * 100)}%");

                Thread.Sleep(500);
            }
        }
    }
}
