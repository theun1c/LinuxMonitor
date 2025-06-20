using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Monitor.Network
{
    public class NetworkMonitor : IMonitor
    {
        readonly ILogger _logger;
        public NetworkMonitor(ILogger logger)
        {
            _logger = logger;
        }
        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);

                string output = await executor.ExecuteLinuxCommandAsync("nslookup nnov.ru");
                var lines = output.Split('\n');

                if (lines.Length >= 2)
                {
                    Match match = Regex.Match(lines[0], @"\.(\d+)$");
                    Match secondMatch = Regex.Match(lines[0], @"Server:\s*(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})");
                    if (match.Success) 
                    {
                        _logger.Info($"[NETWORK] IP: " + secondMatch.Groups[1].Value);
                    }
                }

            }
            catch (Exception ex) 
            {
                _logger.Error($"[NETWORK] Monitoring failed: {ex.Message}");
            }
        }
    }
}
