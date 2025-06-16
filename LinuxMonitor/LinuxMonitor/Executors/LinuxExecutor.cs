using LinuxMonitor.Logging;
using System.Diagnostics;

namespace LinuxMonitor.BashExecutor
{
    public class LinuxExecutor
    {
        private readonly ILogger _logger; 

        public LinuxExecutor(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<string> ExecuteLinuxCommandAsync(string command)
        {
            try
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"{command}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };

                process.Start(); 

                Task<string> outputTask = process.StandardOutput.ReadToEndAsync(); 
                Task<string> errorTask = process.StandardError.ReadToEndAsync(); 

                await process.WaitForExitAsync(); 

                string output = await outputTask;
                string error = await errorTask;

                if (!string.IsNullOrWhiteSpace(error))
                {
                    _logger.Warn($"[warning]: {error}");
                }

                return output; 
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception while executing: {command}"); 
                return string.Empty; 
            }
        }
    }
}
