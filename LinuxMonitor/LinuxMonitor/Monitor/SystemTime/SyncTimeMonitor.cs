using LinuxMonitor.BashExecutor;
using LinuxMonitor.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinuxMonitor.Monitor.SystemTime
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

                string syncOutput = await executor.ExecuteLinuxCommandAsync(syncCommand);

                syncOutput = syncOutput.Trim(); // yes/no sudo systemctl start ntp

                if (syncOutput == "no")
                {
                    _logger.Warn($"[SYNCTIME] Is sync: {syncOutput}");
                }
                else if (syncOutput == "yes") 
                {
                    _logger.Info($"[SYNCTIME] Is sync: {syncOutput}");
                }
                else
                {
                    _logger.Error($"[SYNCTIME] Error");
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
