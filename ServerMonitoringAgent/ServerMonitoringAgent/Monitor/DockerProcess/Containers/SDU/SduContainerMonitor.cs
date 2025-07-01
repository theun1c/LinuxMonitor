using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Executors;
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
        readonly ILinuxExecutor _executor;

        public SduContainerMonitor(ILogger logger, ILinuxExecutor executor)
        {
            _logger = logger;
            _executor = executor;
        }
        public async Task MonitorAsync()
        {
            try
            {
                string command = "docker ps --format \"{{.Names}}\"";
                string containerName = "sdu";

                var output = (await _executor.ExecuteLinuxCommandAsync(command)).Trim();

                if (output.Contains(containerName))
                {
                    _logger.Info("[SDUCON] 1");
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
