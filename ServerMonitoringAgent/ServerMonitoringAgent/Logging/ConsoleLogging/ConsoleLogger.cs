﻿namespace ServerMonitoringAgent.Logging.ConsoleLogging
{
    /// <summary>
    /// ENG: console logger class
    /// RUS: класс консольного логгера 
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void Error(string message) => Write(message, "[ERROR]", ConsoleColor.Red);
        public void Info(string message) => Write(message, "[INFO]", ConsoleColor.Green);
        public void Warn(string message) => Write(message, "[WARN]", ConsoleColor.Yellow);

        //Когда один поток входит в блок lock, другие потоки ждут, пока первый не завершит работу с этим блоком.
        private static readonly object _lock = new object(); // для корректного отображения цветов

        /// <summary>
        /// ENG: the main method of the logger
        /// RUS: главный метод логгера
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="color"></param>
        private void Write(string message, string level, ConsoleColor color)
        { 
            lock (_lock)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine($"{level} {DateTime.Now:HH:mm:ss} - {message}");
                Console.ForegroundColor = foregroundColor;
            }
        }
    }
}
