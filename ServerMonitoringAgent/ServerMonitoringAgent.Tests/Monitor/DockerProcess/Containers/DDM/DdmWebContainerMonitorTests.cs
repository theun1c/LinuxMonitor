using Moq;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using ServerMonitoringAgent.Monitor.DockerProcess.Containers.DDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Tests.Monitor.DockerProcess.Containers.DDM
{
    public class DdmWebContainerMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<ILinuxExecutor> _linuxExecutorMock;
        private readonly DdmWebContainerMonitor _ddmWebConMonitor;

        public DdmWebContainerMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _linuxExecutorMock = new Mock<ILinuxExecutor>();
            _ddmWebConMonitor = new DdmWebContainerMonitor(_loggerMock.Object, _linuxExecutorMock.Object);
        }

        [Fact]
        public async Task DddmWebConMonitor_WhenContainerAvailable_LogsInfoMessage()
        {
            // Arrange
            string validOutput = "... ... ... ...   10 hours ago   Up 10 hours   0.0.0.0:88->80/tcp         ddmweb-ui";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _ddmWebConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("[DDMWEBCON] 1"))), Times.Once);
        }

        [Fact]
        public async Task DddmWebConMonitor_WhenContainerUnavailable_LogsWarnMessage()
        {
            // Arrange
            string validOutput = "unavailable";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _ddmWebConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Warn(It.Is<string>(s => s.Contains("[DDMWEBCON] 0"))), Times.Once);
        }

        [Fact]
        public async Task DddmWebConMonitor_WhenException_LogsErrorMessage()
        {
            // Arrange
            string exceptionMessage = "excep";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            await _ddmWebConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[DDMWEBCON] Monitoring failed: excep"))), Times.Once);
        }
    }
}
