using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using System.Text.RegularExpressions;

namespace ServerMonitoringAgent.Monitor.Storage
{
    /// <summary>
    /// ENG: monitoring class for storage
    /// RUS: класс мониторинга для хранилища
    /// </summary>
    public class StorageMonitor : IMonitor
    {
        readonly ILogger _logger;
        readonly ILinuxExecutor _executor;

        public StorageMonitor(ILogger logger, ILinuxExecutor executor)
        {
            _logger = logger;
            _executor = executor;
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
                string command = "df /";

                string output = await _executor.ExecuteLinuxCommandAsync(command);

                var lines = output.Split('\n');
                if (lines.Length >= 2)
                {
                    var parts = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 5)
                    {
                        string usage = parts[4];
                        Match usageMatch = Regex.Match(usage, @"\d+");
                        if (usageMatch.Success) 
                        {
                            _logger.Info($"[STORAGE] {usageMatch}");
                        }
                        else
                        {
                            _logger.Error("[STORAGE] Parsing failed");
                        }
                    }
                }
                else
                {
                    _logger.Error("[STORAGE] Parsing failed");
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
