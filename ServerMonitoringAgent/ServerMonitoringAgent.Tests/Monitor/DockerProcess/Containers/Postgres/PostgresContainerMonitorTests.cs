using Moq;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using ServerMonitoringAgent.Monitor.DockerProcess.Containers.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Tests.Monitor.DockerProcess.Containers.Postgres
{
    public class PostgresContainerMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<ILinuxExecutor> _linuxExecutorMock;
        private readonly PostgresContainerMonitor _postgresConMonitor;

        public PostgresContainerMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _linuxExecutorMock = new Mock<ILinuxExecutor>();
            _postgresConMonitor = new PostgresContainerMonitor(_loggerMock.Object, _linuxExecutorMock.Object);
        }

        [Fact]
        public async Task PostgresConMonitor_WhenContainerAvailable_LogsInfoMessage() 
        {
            // Arrange
            string validOutput = "... ... ...   5 days ago   Up 5 days             postgres";
            _linuxExecutorMock.Setup(x=> x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _postgresConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("[POSTGRESCON] 1"))), Times.Once);
            
        }

        [Fact]
        public async Task PostgresConMonitor_WhenContainetUnavailable_LogsWarnMessage()
        {
            // Arrange
            string validOutput = "-unavail";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _postgresConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Warn(It.Is<string>(s => s.Contains("[POSTGRESCON] 0"))), Times.Once);

        }

        [Fact]
        public async Task PostgresConMonitor_WhenException_LogsErrorMessage()
        {
            // Arrange
            string exception = "ex";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exception));

            // Act
            await _postgresConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[POSTGRESCON] Monitoring failed: ex"))), Times.Once);

        }
    }
}
