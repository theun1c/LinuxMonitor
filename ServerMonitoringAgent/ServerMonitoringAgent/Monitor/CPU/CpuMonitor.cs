using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Logging;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ServerMonitoringAgent.Monitor.CPU
{
    /// <summary>
    /// ENG: monitoring class for CPU
    /// RUS: класс мониторинга для ЦП
    /// </summary>
    public class CpuMonitor : IMonitor
    {
        readonly ILogger _logger;
        public CpuMonitor(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// ENG: the main monitoring method for CPU
        /// RUS: главный метод мониторинга для ЦП
        /// </summary>
        /// <returns>console output</returns>
        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);

                string output = await executor.ExecuteLinuxCommandAsync("vmstat 1 2");
                var lines = output.Split('\n');
                if (lines.Length >= 2)
                {
                    var parts = lines[3].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 15)
                    {
                        string usage = parts[14];
                        _logger.Info($"[CPU] {100 - Convert.ToInt32(usage)}");
                    }
                }
      
                return;
            }
            catch (Exception ex) 
            {
                _logger.Error($"[CPU] Monitoring failed: {ex.Message}");
            }
        }
    }
}
