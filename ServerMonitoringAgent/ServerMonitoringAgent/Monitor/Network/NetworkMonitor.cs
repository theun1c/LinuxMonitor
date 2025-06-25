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
                string command = "nslookup nnov.ru";

                string output = await executor.ExecuteLinuxCommandAsync(command);
                
                var lines = output.Split('\n');
                if (!lines[0].Contains(";; communications error to"))
                {
                    Match match = Regex.Match(lines[0], @"\.(\d+)$");
                    Match secondMatch = Regex.Match(lines[0], @"Server:\s*(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})");
                    if (match.Success)
                    {
                        if (Convert.ToInt32(match.Groups[1].Value) == 160)
                        {
                            _logger.Info($"[NETWORK] 1");
                        }
                        else
                        {
                            _logger.Warn($"[NETWORK] 0");
                        }
                    }
                }
                else
                {
                    _logger.Warn($"[NETWORK] 0");
                    //_logger.Error($"[NETWORK] Monitoring failed: nslookup communications error");
                }

                return;
            }
            catch (Exception ex)
            {
                _logger.Error($"[NETWORK] Monitoring failed: {ex.Message}");
            }
        }
    }
}
