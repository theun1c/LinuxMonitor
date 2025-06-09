using System.Diagnostics;

namespace LinuxMonitor.BashExecutor
{
    public class LinuxExecutor
    {
        public string Command { get; set; } // вроде бы и юзлесс но пускай будет

        // TODO: добавит логгирование
        // TODO: добавить обработку аутпута
        /// <summary>
        /// ТЕСТОВЫЙ МЕТОД ДЛЯ ВВОДА КОМАНД В БАШ
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string ExecuteLinuxCommand(string command)
        {
            var process = new Process() // создает объект процесса 
            {
                StartInfo = new ProcessStartInfo // настройки запуска внешнего процесса
                {
                    FileName = "/bin/bash", // указывам какую внешнюю оболочку запускаем 
                    Arguments = $"-c \"{command}\"", // типо bash -c "ls -l" 
                    RedirectStandardOutput = true, // ПЕРЕНАПРАВЛЯЕТ ВЫПОЛНЕНИЕ КОМАНДЫЫ БАША ВМЕСТО КОНСОЛИ в код
                    UseShellExecute = false, // отключаем использование оболочки винды !! нужно чтобы работал редирект
                    CreateNoWindow = true, // не показывать окно терминала
                }
            };

            process.Start(); // запускаем процесс
            string output = process.StandardOutput.ReadToEnd(); // считывает stdout
            process.WaitForExit(); // ожидание выполнения

            return output; // просто возвращаем строку
        }
    }
}
