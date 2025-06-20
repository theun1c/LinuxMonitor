using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Logging;

namespace ServerMonitoringAgent.Monitor.Storage
{
    /// <summary>
    /// ENG: monitoring class for storage
    /// RUS: класс мониторинга для хранилища
    /// </summary>
    public class StorageMonitor : IMonitor
    {
        readonly ILogger _logger;
        public StorageMonitor(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// ENG: the main monitoring method for storage
        /// RUS: главный метод мониторинга для хранилища
        /// </summary>
        /// <returns>console output</returns>
        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);

                string output = await executor.ExecuteLinuxCommandAsync("df /");
                var lines = output.Split('\n');
                if (lines.Length >= 2)
                {
                    var parts = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 5)
                    {
                        string usage = parts[4];
                        _logger.Info($"[STORAGE] Disk Usage: {usage}");
                    }
                }

                return;
            }
            catch (Exception ex) 
            {
                _logger.Error($"[STORAGE] Monitoring failed: {ex.Message}");
            }
        }
    }
}
