using LinuxMonitor.BashExecutor;
using LinuxMonitor.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LinuxMonitor.Monitor
{
    public class CpuMonitor : IMonitor
    {
        readonly ILogger _logger;
        public CpuMonitor(ILogger logger)
        {
            _logger = logger;
        }
        public async Task MonitorAsync()
        {
            try
            {
                var executor = new LinuxExecutor(_logger);

                string output = await executor.ExecuteLinuxCommandAsync("top -bn1 | grep \"Cpu\\(s\\)\"");
                var match = Regex.Match(output, @"(\d+\.\d+)\s+id");
                if (match.Success)
                {
                    double idle = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    double usage = 100 - idle;
                    _logger.Info($"[CPU] CPU Usage: {usage:F1}%");
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
