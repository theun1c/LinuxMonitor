using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Executors;
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
        readonly ILinuxExecutor _executor;

        public EtcdContainerMonitor(ILogger logger, ILinuxExecutor executor)
        {
            _logger = logger;
            _executor = executor;
        }

        public async Task MonitorAsync()
        {
            try
            {
                string command = "docker ps --format \"{{.Names}}\"";
                string containerName = "etcd";

                var output = (await _executor.ExecuteLinuxCommandAsync(command)).Trim();

                if (output.Contains(containerName))
                {
                    _logger.Info("[ETCDCON] 1");
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
