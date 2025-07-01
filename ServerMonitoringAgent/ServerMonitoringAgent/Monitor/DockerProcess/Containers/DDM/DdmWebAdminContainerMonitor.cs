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
    public class DdmWebAdminContainerMonitor : IMonitor
    {
        readonly ILogger _logger;
        readonly ILinuxExecutor _executor;
        public DdmWebAdminContainerMonitor(ILogger logger, ILinuxExecutor executor)
        {
            _logger = logger;
            _executor = executor;
        }

        public async Task MonitorAsync()
        {
            try
            {
                string command = "docker ps --format \"{{.Names}}\"";
                string containerName = "ddmwebadmin-ui";
                var output = (await _executor.ExecuteLinuxCommandAsync(command)).Trim();

                if (output.Contains(containerName))
                {
                    _logger.Info("[DDMWEBADMINCON] 1");
                }
                else
                {
                    _logger.Warn("[DDMWEBADMINCON] 0");
                }

                return;
            }
            catch (Exception ex)
            {
                _logger.Error($"[DDMWEBADMINCON] Monitoring failed: {ex.Message}");
            }
        }
    }
}
