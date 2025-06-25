using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Logging;
using System.Text.RegularExpressions;

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
                string command = "free -m";

                var output = await executor.ExecuteLinuxCommandAsync(command);
                
                var lines = output.Split('\n');
                var memLine = lines.FirstOrDefault(l => l.StartsWith("Mem:"));
                if (memLine != null)
                {
                    var parts = memLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 3)
                    {
                        string total = parts[1];
                        Match totalMatch = Regex.Match(total, @"\d+\.?\d*");
                        string used = parts[2];
                        Match usedMatch = Regex.Match(used, @"\d+\.?\d*");

                        if (totalMatch.Success && usedMatch.Success )
                        {
                            double totalVal = double.Parse(totalMatch.Value);
                            double usedVal = double.Parse(usedMatch.Value);
                            double percent = (usedVal / totalVal) * 100;
                            _logger.Info($"[MEMORY] {percent:F0}");
                        }
                        else
                        {
                            _logger.Error("[MEMORY] Converting failed");
                        }
                    }
                }
                else
                {
                    _logger.Error("[MEMORY] Parsing failed");

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
