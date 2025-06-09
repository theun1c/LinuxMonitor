using LinuxMonitor.BashExecutor;

namespace LinuxMonitor
{
    internal class Program
    {
        /// <summary>
        /// точка входа да
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            LinuxExecutor linuxExecutor = new LinuxExecutor(); // потом мб перепишу 
            linuxExecutor.Command = "free -h"; // задаем команлу
            // запускаем поток ОЗУ
            var memoryThread = new Thread(() =>
            {
                while (true) // пока не нажмут ctrl C
                {
                    string output = linuxExecutor.ExecuteLinuxCommand(linuxExecutor.Command); // получаем аутпут команды
                    Console.WriteLine(output); // вывод полученной строки
                    Thread.Sleep(2_000); // задержка печати в 1с
                }
            });
            memoryThread.Start(); // запуск потока


            string command = "top -bn1 | grep \"Cpu\\(s\\)\"";
            var CPUThread = new Thread(() =>
            {
                while (true)
                {
                    string output = linuxExecutor.ExecuteLinuxCommand(command);
                    Console.WriteLine(output);
                    Thread.Sleep(2_000);
                }
            });
            CPUThread.Start();
        }
    }
}
            