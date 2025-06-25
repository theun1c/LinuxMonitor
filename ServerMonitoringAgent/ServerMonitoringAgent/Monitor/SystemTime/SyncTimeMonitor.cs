using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Monitor.SystemTime
{
    public class SyncTimeMonitor : IMonitor
    {
        readonly ILogger _logger;
        public SyncTimeMonitor(ILogger logger)
        {
            _logger = logger;
        }
        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);

                string syncCommand = "timedatectl | awk '/System clock synchronized/ {print $4}'";

                string syncOutput = (await executor.ExecuteLinuxCommandAsync(syncCommand)).Trim();

                if (syncOutput == "no")
                {
                    _logger.Warn($"[SYNCTIME] 0");
                }
                else if (syncOutput == "yes")
                {
                    _logger.Info($"[SYNCTIME] 1");
                }
                else
                {
                    _logger.Error($"[SYNCTIME] Parsing failed");
                }

                return;
            }
            catch (Exception ex)
            {
                _logger.Error($"[SYNCTIME] Monitoring failed: {ex.Message}");
            }
        }
    }
}
