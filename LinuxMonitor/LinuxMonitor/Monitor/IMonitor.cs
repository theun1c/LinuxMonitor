using LinuxMonitor.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinuxMonitor.Monitor
{
    public interface IMonitor
    {
        Task MonitorAsync();
    }
}
