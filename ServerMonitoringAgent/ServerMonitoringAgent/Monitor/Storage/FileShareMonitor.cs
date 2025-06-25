using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Monitor.Storage
{
    public class FileShareMonitor : IMonitor
    {
        readonly ILogger _logger;
        public FileShareMonitor(ILogger logger)
        {
            _logger = logger;   
        }
        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);
                string command = "df -h";
                string output = await executor.ExecuteLinuxCommandAsync(command);

                if (output.Contains("//10.10.130.96/share")) 
                {
                    _logger.Info("[FILESHARE] 1");
                }
                else
                {
                    _logger.Warn("[FILESHARE] 0");
                }

                return;
            }
            catch (Exception ex) 
            {
                _logger.Error($"[FILESHARE] Monitoring failed: {ex.Message}");
            }
        }
    }
}
