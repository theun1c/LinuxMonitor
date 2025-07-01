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
    public class DdmWebApiContainerMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<ILinuxExecutor> _linuxExecutorMock;
        private readonly DdmWebApiContainerMonitor _ddmWebApiConMonitor;

        public DdmWebApiContainerMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _linuxExecutorMock = new Mock<ILinuxExecutor>();
            _ddmWebApiConMonitor = new DdmWebApiContainerMonitor(_loggerMock.Object, _linuxExecutorMock.Object);
        }

        [Fact]
        public async Task DdmWebApiConMonitor_WhenContainerAvailable_LogsInfoMessage()
        {
            // Arrange
            string validOutput = "... ... ... ddmweb-api-astra:5.22.4.4       \"sh start.sh\"         10 hours ago   Up 2 hours    0.0.0.0:44365->44365/tcp   ddmwebapi";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _ddmWebApiConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("[DDMWEBAPICON] 1"))), Times.Once);

        }

        [Fact]
        public async Task DdmWebApiConMonitor_WhenContainerUnavailable_LogsWarnMessage()
        {
            // Arrange
            string validOutput = "";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _ddmWebApiConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Warn(It.Is<string>(s => s.Contains("[DDMWEBAPICON] 0"))), Times.Once);
        }

        [Fact]
        public async Task DdmWebApiConMonitor_WhenException_LogsErrorMessage()
        {
            // Arrange
            string exceptionMessage = "ex";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            await _ddmWebApiConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[DDMWEBAPICON] Monitoring failed: ex"))), Times.Once);
        }
    }
}
