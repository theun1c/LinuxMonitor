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
            string cpuInfo = ExecuteLinuxCommand("cat /proc/stat | head -n 1");
            Console.WriteLine(cpuInfo); // cpu  1234 567 890 123456 0 0 0 0 0 0
        }

        public static string ExecuteLinuxCommand(string command)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }
    }
}
