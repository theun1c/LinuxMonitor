using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Logging;

namespace ServerMonitoringAgent.Monitor.Memory
{
    /// <summary>
    /// ENG: monitoring class for memory
    /// RUS: класс мониторинга для памяти
    /// </summary>
    public class MemoryMonitor : IMonitor
    {
        readonly ILogger _logger;
        public MemoryMonitor(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// ENG: the main monitoring method for memory
        /// RUS: главный метод мониторинга для памяти
        /// </summary>
        /// <returns>console output</returns>
        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);

                string output = await executor.ExecuteLinuxCommandAsync("free -h");
                var lines = output.Split('\n');
                var memLine = lines.FirstOrDefault(l => l.StartsWith("Mem:"));
                if (memLine != null)
                {
                    var parts = memLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 3)
                    {
                        string total = parts[1];
                        string used = parts[2];
                        _logger.Info($"[MEMORY] RAM Used: {used} / {total}");
                    }
                }

                return;
            }
            catch (Exception ex) 
            {
                _logger.Error($"[MEMORY] Monitoring failed: {ex.Message}");
            }
        }
    }
}
