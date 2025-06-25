using ServerMonitoringAgent.BashExecutor;
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
        public DdmWebContainerMonitor(ILogger logger)
        {
            _logger = logger;
        }

        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);

                var output = (await executor.ExecuteLinuxCommandAsync("docker ps")).Trim();

                if (output.Contains("ddmweb-ui"))
                {
                    _logger.Error("[DDMWEBCON] 1");
                }
                else
                {
                    _logger.Warn("[DDMWEBCON] 0");
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
