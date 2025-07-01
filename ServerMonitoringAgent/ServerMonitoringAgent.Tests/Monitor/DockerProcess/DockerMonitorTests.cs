using Moq;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using ServerMonitoringAgent.Monitor.CPU;
using ServerMonitoringAgent.Monitor.DockerProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Tests.Monitor.DockerProcess
{
    public class DockerMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly DockerMonitor _dockerMonitor;
        private readonly Mock<ILinuxExecutor> _linuxExecutorMock;

        public DockerMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _linuxExecutorMock = new Mock<ILinuxExecutor>();
            _dockerMonitor = new DockerMonitor(_loggerMock.Object, _linuxExecutorMock.Object);
        }

        [Fact]
        public async Task DockerMonitor_WhenOutputValid_LogsCorrectDockerАvailability()
        {
            // Arrange
            string validDockerOutput = "Active: active (running) since Sun 2025-04-20 22:31:10 MSK; 2 months 2 days ago";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validDockerOutput);

            // Act
            await _dockerMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("[DOCKER] 1"))), Times.Once);
        }

        [Fact]
        public async Task DockerMonitor_WhenDockerInactive_LogsWarnMEssage()
        {
            // Arrange
            string inactiveDockerOutput = "Active: inactive (stopped)";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(inactiveDockerOutput);

            // Act
            await _dockerMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Warn(It.Is<string>(s => s.Contains("[DOCKER] 0"))), Times.Once);
        }

        [Fact]
        public async Task DockerMonitor_WhenException_LogsErrorMessage()
        {
            // Arrange
            string exceptionMessage = "Smth failed";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            await _dockerMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[DOCKER] Monitoring failed: Smth failed"))), Times.Once);
        }

    }
}
