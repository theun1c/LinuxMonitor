using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Monitor.DockerProcess
{
    public class DockerMonitor : IMonitor
    {
        readonly ILogger _logger;
        readonly ILinuxExecutor _executor;

        public DockerMonitor(ILogger logger, ILinuxExecutor executor)
        {
            _logger = logger;
            _executor = executor;
        }

        public async Task MonitorAsync()
        {
            try
            {
                string command = "systemctl status docker | grep \"Active:\" ";

                var output = (await _executor.ExecuteLinuxCommandAsync(command)).Trim();

                if (output.Contains("active (running)")) 
                {
                    _logger.Info("[DOCKER] 1");
                }
                else
                {
                    _logger.Warn("[DOCKER] 0");
                }

                return;
            }
            catch (Exception ex) 
            {
                _logger.Error($"[DOCKER] Monitoring failed: {ex.Message}");
            }
        }
    }
}
