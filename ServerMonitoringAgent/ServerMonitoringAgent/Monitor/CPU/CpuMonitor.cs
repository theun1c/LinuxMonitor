using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Executors;
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
        readonly ILinuxExecutor _executor;
        public CpuMonitor(ILogger logger, ILinuxExecutor executor)
        {
            _logger = logger;
            _executor = executor;
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
                string command = "vmstat 1 2";

                string output = await _executor.ExecuteLinuxCommandAsync(command);
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
                else
                {
                    _logger.Error("[CPU] Parsing failed");
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
