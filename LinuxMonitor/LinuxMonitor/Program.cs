using LinuxMonitor.BashExecutor;
using LinuxMonitor.Logging;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace LinuxMonitor
{
    internal class Program
    {
        /// <summary>
        /// мейн с асинком
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            // использую канселешн токен для обработки сочетания ктрл + Ц в консольке
            var cts = new CancellationTokenSource(); // новый объект токена
            // s - sender e - event args
            Console.CancelKeyPress += (s, e) => // подписка на событие Кенсен Кей Пресс
            {
                e.Cancel = true; // отмена стандартного поведения (завершения приложения)
                cts.Cancel(); // запуск отмены операций через CancellationTokenSource !!! механизм кооперативной отмены
                // операции с токеном получают уведомление, что нужно прекратить работу
            };

            var logger = new ConsoleLogger(); // создание консольного логгера 

            // запуск мониторинга
            await Task.WhenAll(
                // КОМАНДА МЕТКА ЛОГГЕР ТОКЕН
                MonitorAsync("free -h", "[MEMORY]", logger, cts.Token),
                MonitorAsync("top -bn1 | grep \"Cpu\\(s\\)\"", "[CPU]", logger, cts.Token),
                MonitorAsync("df /", "[STORAGE]", logger, cts.Token)
            );

        }

        /// <summary>
        /// асинхронный метод мониторинга. 
        /// </summary>
        /// <param name="command"> КОМАНДА, которую следует исполнить в баше </param>
        /// <param name="label"> Метка, указывает на то, для чего команда используется </param>
        /// <param name="logger"> Логгер, необходимый для логгирования </param>
        /// <param name="cancellationToken"> Токен завершения действий </param>
        /// <returns></returns>
        static async Task MonitorAsync(string command, string label, ILogger logger, CancellationToken cancellationToken)
        {
            var executor = new LinuxExecutor(logger); // создание объекта экзекутора с передачей токена
                                                      //while (!cancellationToken.IsCancellationRequested) // пока не пришло уведомление о завершении 
                                                      //{
            if (label == "[MEMORY]")
            {
                string output = executor.ExecuteLinuxCommand(command);
                var lines = output.Split('\n');
                var memLine = lines.FirstOrDefault(l => l.StartsWith("Mem:"));
                if (memLine != null)
                {
                    var parts = memLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 3)
                    {
                        string total = parts[1];
                        string used = parts[2];
                        logger.Info($"{label} RAM Used: {used} / {total}");
                    }
                }
            }
            else if (label == "[CPU]")
            {
                string output = executor.ExecuteLinuxCommand(command);
                var match = Regex.Match(output, @"(\d+\.\d+)\s+id");
                if (match.Success)
                {
                    double idle = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    double usage = 100 - idle;
                    logger.Info($"{label} CPU Usage: {usage:F1}%");
                }
            }
            else
            {
                string output = executor.ExecuteLinuxCommand(command);
                var lines = output.Split('\n');
                if (lines.Length >= 2)
                {
                    var parts = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 5)
                    {
                        //string available = parts[3];
                        string usage = parts[4];
                        logger.Info($"{label} Disk Usage: {usage}");
                    }
                }
            }
            //logger.Info($"{label} output:\n{output}"); // с помощью логгер авыводим информацию в консоль

            //    try
            //    {
            //        await Task.Delay(5_000, cancellationToken); // задержка в милисеках
            //                                                    // поставить 300 _000 для 5 минут
            //    }
            //    catch (TaskCanceledException) // если программа завершилась при ктрл Ц (с помощью токена)
            //    {
            //        logger.Warn($"{label} monitoring cancelled"); // вывод предупреждения (желтый) в консоль
            //        break;
            //    }
            //}
            logger.Info($"{label} monitoring finished"); // отчет об успешном завершении мониторинга

            
        }
    }
}
