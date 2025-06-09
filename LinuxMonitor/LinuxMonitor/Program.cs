using LinuxMonitor.BashExecutor;
using LinuxMonitor.Logging;
using System.Runtime.CompilerServices;

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
                MonitorAsync("free -h", "[MEM]", logger, cts.Token),
                MonitorAsync("top -bn1 | grep \"Cpu\\(s\\)\"", "[CPU]", logger, cts.Token)
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
            while (!cancellationToken.IsCancellationRequested) // пока не пришло уведомление о завершении 
            {
                string output = executor.ExecuteLinuxCommand(command); // вывод 
                logger.Info($"{label} output:\n{output}"); // с помощью логгер авыводим информацию в консоль

                try
                {
                    await Task.Delay(2_000, cancellationToken); // задержка в 2 секунды на выполнение 
                }
                catch (TaskCanceledException) // если программа завершилась при ктрл Ц (с помощью токена)
                {
                    logger.Warn($"{label} monitoring cancelled"); // вывод предупреждения (желтый) в консоль
                    break; 
                }
            }
            logger.Info($"{label} monitoring finished"); // отчет об успешном завершении мониторинга
        }
    }
}
            