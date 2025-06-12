using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinuxMonitor.Logging
{
    /// <summary>
    /// класс логгера для консоли. наследуется от базового интерфейса
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        // реализация интерфейса
        public void Error(string message) => Write(message, "[ERROR]", ConsoleColor.Red);
        public void Info(string message) => Write(message, "[INFO]", ConsoleColor.Green);
        public void Warn(string message) => Write(message, "[WARN]", ConsoleColor.Yellow);
        //public void DebuggingLog(string message) => Write(message, "[DEGUG]", ConsoleColor.Cyan);

        /// <summary>
        /// главный метод вывода в консоль
        /// </summary>
        /// <param name="message">сообщение, которое нужно вывести в консоль</param>
        /// <param name="level">уровень предупреждения ("опасности"). все хорошо/все плохо/предупреждение</param>
        /// <param name="color">цвет консоли при разных уровнях</param>
        private void Write(string message, string level, ConsoleColor color)
        { 
            var foregroundColor = Console.ForegroundColor; // цвет консоли по умолчанию
            Console.ForegroundColor = color; // красим в цвет, исходя из уровня
            Console.WriteLine($"{level} {DateTime.Now:HH:mm:ss} - {message}"); // вывод в консоль "УРОВЕНЬ / ДАТАВРЕМЯ / СООБЩЕНИЕ"
            Console.ForegroundColor = foregroundColor; // обратно меняем цвет консоли на дефолтный
        }
    }
}
