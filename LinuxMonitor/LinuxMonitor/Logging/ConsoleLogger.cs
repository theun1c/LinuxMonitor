using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinuxMonitor.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Error(string message) => Write(message, "[ERROR]", ConsoleColor.Red);
        public void Info(string message) => Write(message, "[INFO]", ConsoleColor.Green);
        public void Warn(string message) => Write(message, "[WARN]", ConsoleColor.Yellow);

        private void Write(string message, string level, ConsoleColor color)
        { 
            var foregroundColor = Console.ForegroundColor; 
            Console.ForegroundColor = color; 
            Console.WriteLine($"{level} {DateTime.Now:HH:mm:ss} - {message}"); 
            Console.ForegroundColor = foregroundColor;
        }
    }
}
