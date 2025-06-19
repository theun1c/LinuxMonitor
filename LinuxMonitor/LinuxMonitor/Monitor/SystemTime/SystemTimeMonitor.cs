using LinuxMonitor.BashExecutor;
using LinuxMonitor.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinuxMonitor.Monitor.SystemTime
{
    public class SystemTimeMonitor : IMonitor
    {
        readonly ILogger _logger;
        public SystemTimeMonitor(ILogger logger)
        {
            _logger = logger;
        }

        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);

                string dateCommand = "date -u '+%a %Y-%m-%d %H:%M:%S UTC'";

                string dateOutput = await executor.ExecuteLinuxCommandAsync(dateCommand);

                dateOutput = dateOutput.Trim(); // Thu 2025-06-19 01:51:18 UTC

                string systemTime = DateTime.UtcNow.ToString("ddd yyyy-MM-dd HH:mm:ss 'UTC'");

                if (systemTime == dateOutput)
                {
                    _logger.Info($"[SYSTIME] UTC: {dateOutput}");
                }

                return;
            }
            catch (Exception ex) 
            {
                _logger.Error($"[SYSTIME] Monitoring failed: {ex.Message}");
            }
        }
    }
}
