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
    public class DdmWebApiContainerMonitor : IMonitor
    {
        readonly ILogger _logger;
        readonly ILinuxExecutor _executor;
        public DdmWebApiContainerMonitor(ILogger logger, ILinuxExecutor executor)
        {
            _logger = logger;
            _executor = executor;
        }

        public async Task MonitorAsync()
        {
            try
            {
                string command = "docker ps --format \"{{.Names}}\"";
                string containerName = "ddmwebapi";

                var output = (await _executor.ExecuteLinuxCommandAsync(command)).Trim();

                if (output.Contains(containerName))
                {
                    _logger.Info("[DDMWEBAPICON] 1");
                }
                else
                {
                    _logger.Warn("[DDMWEBAPICON] 0");
                }

                return;
            }
            catch (Exception ex)
            {
                _logger.Error($"[DDMWEBAPICON] Monitoring failed: {ex.Message}");
            }
        }
    }
}
