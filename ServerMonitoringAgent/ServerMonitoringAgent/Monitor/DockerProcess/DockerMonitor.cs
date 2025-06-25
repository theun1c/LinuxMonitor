using ServerMonitoringAgent.BashExecutor;
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
        public DockerMonitor(ILogger logger)
        {
            _logger = logger;
        }

        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);

                var output = (await executor.ExecuteLinuxCommandAsync("systemctl status docker | grep \"Active:\" ")).Trim();

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
