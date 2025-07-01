using Moq;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using ServerMonitoringAgent.Monitor.Network;
using ServerMonitoringAgent.Monitor.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Tests.Monitor.Storage
{
    public class FileShareMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly FileShareMonitor _fileShareMonitor;
        private readonly Mock<ILinuxExecutor> _linuxExecutorMock;

        public FileShareMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _linuxExecutorMock = new Mock<ILinuxExecutor>();
            _fileShareMonitor = new FileShareMonitor(_loggerMock.Object, _linuxExecutorMock.Object);
        }

        [Fact]
        public async Task FileShareMonitor_WhenFileShareExists_LogsInfoMessage()
        {
            // Arrange
            string validOutput = "//10.10.130.96/share";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _fileShareMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Info(It.Is<string>(s=> s.Contains("[FILESHARE] 1"))), Times.Once);
        }

        [Fact]
        public async Task FileShareMonitor_WhenFileShareIsNotExists_LogsWarnMessage()
        {
            // Arrange
            string validOutput = " ";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _fileShareMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Warn(It.Is<string>(s => s.Contains("[FILESHARE] 0"))), Times.Once);
        }

        [Fact]
        public async Task FileShareMonitor_WhenException_LogsErrorMessage()
        {
            // Arrange
            string exception = "new ex";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exception));

            // Act
            await _fileShareMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[FILESHARE] Monitoring failed: new ex"))), Times.Once);
        }
    }
}
