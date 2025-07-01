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
    public class DdmWebAdminContainerMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<ILinuxExecutor> _linuxExecutorMock;
        private readonly DdmWebAdminContainerMonitor _ddmWebAdmConMonitor;

        public DdmWebAdminContainerMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _linuxExecutorMock = new Mock<ILinuxExecutor>();
            _ddmWebAdmConMonitor = new DdmWebAdminContainerMonitor(_loggerMock.Object, _linuxExecutorMock.Object);
        }

        [Fact]
        public async Task DdmWebAdmConMonitor_WhenContainerAvailable_LogsInfoMessage()
        {
            // Arrange
            string validOutput = "... ... ... ...       ddmwebadmin-ui";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _ddmWebAdmConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("[DDMWEBADMINCON] 1"))), Times.Once);
        }

        [Fact]
        public async Task DdmWebAdmConMonitor_WhenContainerUnavailable_LogsWarnMessage()
        {
            // Arrange
            string unavailableOutput = "";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(unavailableOutput);

            // Act
            await _ddmWebAdmConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Warn(It.Is<string>(s => s.Contains("[DDMWEBADMINCON] 0"))), Times.Once);
        }

        [Fact]
        public async Task DdmWebAdmConMonitor_WhenException_LogsErrorMessage()
        {
            // Arrange
            string exceptionMessage = "exception ...";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            await _ddmWebAdmConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[DDMWEBADMINCON] Monitoring failed: exception ..."))), Times.Once);
        }
    }
}
