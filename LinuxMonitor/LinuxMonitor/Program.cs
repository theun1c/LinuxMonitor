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
            var output = linuxExecutor.ExecuteLinuxCommand(linuxExecutor.Command); // передаем
            Console.WriteLine(output); // выводим
        }
    }
}
            