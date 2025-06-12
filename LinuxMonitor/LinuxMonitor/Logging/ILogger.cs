using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinuxMonitor.Logging
{
    /// <summary>
    /// базовый интерфейс логгера
    /// </summary>
    public interface ILogger
    {
        // 3 уровня для логгирования
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        //// для отладки надо
        //void DebuggingLog(string message);
    }
}
