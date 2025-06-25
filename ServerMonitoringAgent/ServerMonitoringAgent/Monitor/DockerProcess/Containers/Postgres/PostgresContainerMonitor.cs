using ServerMonitoringAgent.BashExecutor;
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
        public PostgresContainerMonitor(ILogger logger)
        {
            _logger = logger;
        }

        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);
                string command = "docker ps --format \"{{.Names}}\"";
                string containerName = "postgres";

                var output = (await executor.ExecuteLinuxCommandAsync(command)).Trim();

                if (output.Contains(containerName))
                {
                    _logger.Error("[POSTGRESCON] 1");
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
