using Moq;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using ServerMonitoringAgent.Monitor.Memory;
using ServerMonitoringAgent.Monitor.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Tests.Monitor.Network
{
    public class NetworkMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly NetworkMonitor _networkMonitor;
        private readonly Mock<ILinuxExecutor> _linuxExecutorMock;

        public NetworkMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _linuxExecutorMock = new Mock<ILinuxExecutor>();
            _networkMonitor = new NetworkMonitor(_loggerMock.Object, _linuxExecutorMock.Object);
        }

        [Fact]
        public async Task NetworkMonitor_WhenAvailablePort_LogsInfoMessage()
        {
            // Arrange
            string validOutput = "12.12.12.160";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _networkMonitor.MonitorAsync();

            _loggerMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("[NETWORK] 1"))), Times.Once);
        }

        [Fact]
        public async Task NetworkMonitor_WhenUnavailablePort_LogsWarnMessage()
        {
            // Arrange
            string validOutput = "12.12.12.12";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _networkMonitor.MonitorAsync();

            _loggerMock.Verify(l => l.Warn(It.Is<string>(s => s.Contains("[NETWORK] 0"))), Times.Once);
        }

        [Fact]
        public async Task NetworkMonitor_WhenCommunicationWarning_LogsWarnMessage()
        {
            // Arrange
            string validOutput = ";; communications error to";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _networkMonitor.MonitorAsync();

            _loggerMock.Verify(l => l.Warn(It.Is<string>(s => s.Contains("[NETWORK] 0"))), Times.Once);
        }

        [Fact]
        public async Task NetworkMonitor_WhenException_LogsErrorMessage()
        {
            // Arrange
            string exception = "ex";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exception));

            // Act
            await _networkMonitor.MonitorAsync();

            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[NETWORK] Monitoring failed: ex"))), Times.Once);
        }
    }
}
