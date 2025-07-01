using Moq;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using ServerMonitoringAgent.Monitor.DockerProcess.Containers.Postgres;
using ServerMonitoringAgent.Monitor.DockerProcess.Containers.SDU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Tests.Monitor.DockerProcess.Containers.SDU
{
    public class SduContainerMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<ILinuxExecutor> _linuxExecutorMock;
        private readonly SduContainerMonitor _sduConMonitor;

        public SduContainerMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _linuxExecutorMock = new Mock<ILinuxExecutor>();
            _sduConMonitor = new SduContainerMonitor(_loggerMock.Object, _linuxExecutorMock.Object);
        }

        [Fact]
        public async Task SduContainerMonitor_WhenContainerAvailable_LogsInfoMessage()
        {
            // Arrange
            string validOutput = "... ... ... ...            16 minutes ago   Up 16 minutes          0.0.0.0:54545->5050/tcp    sdu";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _sduConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("[SDUCON] 1"))), Times.Once);
        }

        [Fact]
        public async Task SduContainerMonitor_WhenContainerUnavailable_LogsWarnMessage()
        {
            // Arrange
            string validOutput = " ";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _sduConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Warn(It.Is<string>(s => s.Contains("[SDUCON] 0"))), Times.Once);
        }

        [Fact]
        public async Task SduContainerMonitor_WhenException_LogsErrorMessage()
        {
            // Arrange
            string exception = "ex";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exception));

            // Act
            await _sduConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[SDUCON] Monitoring failed: ex"))), Times.Once);
        }
    }
}
