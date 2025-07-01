using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Monitor.DockerProcess.Containers.Postgres
{
    public class PostgresContainerMonitor : IMonitor
    {
        readonly ILogger _logger;
        readonly ILinuxExecutor _executor;

        public PostgresContainerMonitor(ILogger logger, ILinuxExecutor executor)
        {
            _logger = logger;
            _executor = executor;
        }

        public async Task MonitorAsync()
        {
            try
            {
                string command = "docker ps --format \"{{.Names}}\"";
                string containerName = "postgres";

                var output = (await _executor.ExecuteLinuxCommandAsync(command)).Trim();

                if (output.Contains(containerName))
                {
                    _logger.Info("[POSTGRESCON] 1");
                }
                else
                {
                    _logger.Warn("[POSTGRESCON] 0");
                }

                return;
            }
            catch (Exception ex)
            {
                _logger.Error($"[POSTGRESCON] Monitoring failed: {ex.Message}");
            }
        }
    }
}
