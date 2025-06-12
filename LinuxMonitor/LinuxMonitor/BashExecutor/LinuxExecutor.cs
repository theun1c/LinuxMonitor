using LinuxMonitor.Logging;
using System.Diagnostics;

namespace LinuxMonitor.BashExecutor
{
    /// <summary>
    /// класс исполнителя команд
    /// </summary>
    public class LinuxExecutor
    {
        private readonly ILogger _logger; // инициализация логгера

        // конструктор с передачей логгера 
        public LinuxExecutor(ILogger logger)
        {
            _logger = logger;
        }

        // TODO: добавить обработку аутпута
        /// <summary>
        ///  МЕТОД ДЛЯ ВВОДА КОМАНД В БАШ
        /// </summary>
        /// <param name="command">команда, которую следует выполнить</param>
        /// <returns>аутпут</returns>
        public string ExecuteLinuxCommand(string command)
        {
            try
            {
                var process = new Process() // создает объект процесса 
                {
                    StartInfo = new ProcessStartInfo // настройки запуска внешнего процесса
                    {
                        FileName = "/bin/bash", // указывам какую внешнюю оболочку запускаем 
                        Arguments = $"-c \"{command}\"", // типо bash -c "ls -l" 
                        RedirectStandardOutput = true, // ПЕРЕНАПРАВЛЯЕТ ВЫПОЛНЕНИЕ КОМАНДЫЫ БАША ВМЕСТО КОНСОЛИ в код
                        RedirectStandardError = true, // тож самое, но только для ошибок
                        UseShellExecute = false, // отключаем использование оболочки винды !! нужно чтобы работал редирект
                        CreateNoWindow = true, // не показывать окно терминала
                    }
                };

                _logger.Info($"Executing command: {command}"); // указывает о начале выполнении команды
                process.Start(); // запускаем процесс

                string output = process.StandardOutput.ReadToEnd(); // считывает stdout
                string error = process.StandardError.ReadToEnd(); // считывает стандарт еррор
                
                process.WaitForExit(); // ожидание выполнения

                // если еррор поймал ошибку - то выведется варнинг
                if(!string.IsNullOrWhiteSpace(error))
                {
                    _logger.Warn($"[output error warning]: {error}");
                }

                //string[] word = output.Split(' ');
                //foreach (var s in word) 
                //{
                //    if (s != "")
                //        _logger.DebuggingLog(s);
                //}

                return output; // возврат аутпута 
            }
            catch (Exception ex) 
            {
                _logger.Error($"Exception while executing: {command}"); // вывод сведений об ошибке (красный цвет)
                return string.Empty; // возврат пустой строки
            }
        }
    }
}
