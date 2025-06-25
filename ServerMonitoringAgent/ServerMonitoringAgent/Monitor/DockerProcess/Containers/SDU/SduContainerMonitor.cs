using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Monitor.DockerProcess.Containers.SDU
{
    public class SduContainerMonitor : IMonitor
    {
        readonly ILogger _logger;
        public SduContainerMonitor(ILogger logger)
        {
            _logger = logger;
        }
        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);

                var output = (await executor.ExecuteLinuxCommandAsync("docker ps")).Trim();

                if (output.Contains("sdu"))
                {
                    _logger.Error("[SDUCON] 1");
                }
                else
                {
                    _logger.Warn("[SDUCON] 0");
                }

                return;
            }
            catch (Exception ex) 
            {
                _logger.Error($"[SDUCON] Monitoring failed: {ex.Message}");
            }
        }
    }
}
