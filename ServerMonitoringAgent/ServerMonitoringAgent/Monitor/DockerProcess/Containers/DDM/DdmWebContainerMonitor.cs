using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Monitor.DockerProcess.Containers.DDM
{
    public class DdmWebContainerMonitor : IMonitor
    {
        readonly ILogger _logger;
        readonly ILinuxExecutor _executor;
        public DdmWebContainerMonitor(ILogger logger, ILinuxExecutor executor)
        {
            _logger = logger;
            _executor = executor;
        }

        public async Task MonitorAsync()
        {
            try
            {
                string command = "docker ps --format \"{{.Names}}\"";
                string containerName = "ddmweb-ui";

                var output = (await _executor.ExecuteLinuxCommandAsync(command)).Trim();

                if (output.Contains(containerName))
                {
                    _logger.Info("[DDMWEBCON] 1");
                }
                else
                {
                    _logger.Warn("[DDMWEBCON] 0");
                }

                return;
            }
            catch (Exception ex)
            {
                _logger.Error($"[DDMWEBCON] Monitoring failed: {ex.Message}");
            }
        }
    }
}
