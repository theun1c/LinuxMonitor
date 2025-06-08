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
            var thread1 = new Thread(ForTest); // создаем поток
            thread1.Start(1); // запускаем с передачей параметра 
        }

        /// <summary>
        /// функция для запуска в потоке
        /// </summary>
        /// <param name="data"></param>
        private static void ForTest(object data) 
        { 
            int threadNum = (int)data; // преобразование типа даты 
            Console.WriteLine($"Hi from thread #{threadNum}"); // просто вывод в консольку
        }
    }
}
