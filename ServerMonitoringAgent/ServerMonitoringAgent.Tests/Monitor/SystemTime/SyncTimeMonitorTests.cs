using Moq;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using ServerMonitoringAgent.Monitor.Storage;
using ServerMonitoringAgent.Monitor.SystemTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Tests.Monitor.SystemTime
{
    public class SyncTimeMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly SyncTimeMonitor _syncTimeMonitor;
        private readonly Mock<ILinuxExecutor> _linuxExecutorMock;

        public SyncTimeMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _linuxExecutorMock = new Mock<ILinuxExecutor>();
            _syncTimeMonitor = new SyncTimeMonitor(_loggerMock.Object, _linuxExecutorMock.Object);
        }

        [Fact]
        public async Task SyncTimeMonitor_WhenTimeSynchronized_LogsInfoMessage()
        {
            // Arrange
            string validOutput = "yes";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _syncTimeMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("[SYNCTIME] 1"))), Times.Once);
        }

        [Fact]
        public async Task SyncTimeMonitor_WhenTimeUnsynchronized_LogsWarnMessage()
        {
            // Arrange
            string validOutput = "no";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _syncTimeMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Warn(It.Is<string>(s => s.Contains("[SYNCTIME] 0"))), Times.Once);
        }

        [Fact]
        public async Task SyncTimeMonitor_WhenInvalidOutput_LogsErrorMessage()
        {
            // Arrange
            string validOutput = "   ";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _syncTimeMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[SYNCTIME] Parsing failed"))), Times.Once);
        }

        [Fact]
        public async Task SyncTimeMonitor_WhenException_LogsErrorMessage()
        {
            // Arrange
            string exception = "ex";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exception));

            // Act
            await _syncTimeMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[SYNCTIME] Monitoring failed: ex"))), Times.Once);
        }
    }
}
