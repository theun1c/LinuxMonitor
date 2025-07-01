using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Executors
{
    public interface ILinuxExecutor
    {
        Task<string> ExecuteLinuxCommandAsync(string command);
    }
}
