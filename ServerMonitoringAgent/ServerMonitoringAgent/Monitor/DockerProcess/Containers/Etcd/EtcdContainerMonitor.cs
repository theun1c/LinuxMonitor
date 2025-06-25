using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Monitor.DockerProcess.Containers.Etcd
{
    public class EtcdContainerMonitor : IMonitor
    {
        readonly ILogger _logger;

        public EtcdContainerMonitor(ILogger logger)
        {
            _logger = logger;
        }

        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);
                string command = "docker ps --format \"{{.Names}}\"";
                string containerName = "etcd";

                var output = (await executor.ExecuteLinuxCommandAsync(command)).Trim();

                if (output.Contains(containerName))
                {
                    _logger.Error("[ETCDCON] 1");
                }
                else
                {
                    _logger.Warn("[ETCDCON] 0");
                }

                return;
            }
            catch (Exception ex)
            {
                _logger.Error($"[ETCDCON] Monitoring failed: {ex.Message}");
            }
        }
    }
}
