using Moq;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using ServerMonitoringAgent.Monitor.SystemTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Tests.Monitor.SystemTime
{
    public class SystemTimeMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly SystemTimeMonitor _sysTimeMonitor;
        private readonly Mock<ILinuxExecutor> _linuxExecutorMock;

        public SystemTimeMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _linuxExecutorMock = new Mock<ILinuxExecutor>();
            _sysTimeMonitor = new SystemTimeMonitor(_loggerMock.Object, _linuxExecutorMock.Object);
        }

        [Fact]
        public async Task SysTimeMonitor_WhenDateTimeSynchronized_LogsInfoMessage()
        {
            // Arrange
            string dateTimeOutput = DateTime.UtcNow.AddSeconds(-5).ToString("dd.MM.yyyy HH:mm:ss");
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(dateTimeOutput);

            // Act
            await _sysTimeMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("[SYSTIME] 1"))), Times.Once);
        }

        [Fact]
        public async Task SysTimeMonitor_WhenDateTimeUnsynchronized_LogsWarnMessage()
        {
            // Arrange
            string dateTimeOutput = DateTime.UtcNow.AddSeconds(-50).ToString("dd.MM.yyyy HH:mm:ss");
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(dateTimeOutput);

            // Act
            await _sysTimeMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Warn(It.Is<string>(s => s.Contains("[SYSTIME] 0"))), Times.Once);
        }

        [Fact]
        public async Task SysTimeMonitor_WhenException_LogsErrorMessage()
        {
            // Arrange
            string exception = "ex";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exception));

            // Act
            await _sysTimeMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[SYSTIME] Monitoring failed: ex"))), Times.Once);
        }
    }
}
