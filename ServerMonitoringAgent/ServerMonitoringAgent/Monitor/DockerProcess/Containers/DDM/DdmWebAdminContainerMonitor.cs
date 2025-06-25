using ServerMonitoringAgent.BashExecutor;
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
        public DdmWebAdminContainerMonitor(ILogger logger)
        {
            _logger = logger;
        }

        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);

                var output = (await executor.ExecuteLinuxCommandAsync("docker ps --format \"{{.Names}}\"")).Trim();

                if (output.Contains("ddmwebadmin-ui"))
                {
                    _logger.Error("[DDMWEBADMINCON] 1");
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
