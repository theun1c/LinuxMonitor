using Moq;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using ServerMonitoringAgent.Monitor.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Tests.Monitor.Storage
{
    public class StorageMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly StorageMonitor _storageMonitor;
        private readonly Mock<ILinuxExecutor> _linuxExecutorMock;

        public StorageMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _linuxExecutorMock = new Mock<ILinuxExecutor>();
            _storageMonitor = new StorageMonitor(_loggerMock.Object, _linuxExecutorMock.Object);
        }

        [Fact]
        public async Task StorageMonitor_WhenOutputValid_LogsInfoMessage()
        {
            // Arrange
            string validOutput = "Filesystem     1K-blocks     Used Available Use% Mounted on\r\n/dev/sda2       25623780 15600484   8696348  65% /\r\n";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _storageMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("[STORAGE] 65"))), Times.Once);
        }

        [Fact]
        public async Task StorageMonitor_WhenOutputInvalid_LogsErrorMessage()
        {
            // Arrange
            string invalidOutput = "S";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(invalidOutput);

            // Act
            await _storageMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[STORAGE] Parsing failed"))), Times.Once);
        }

        [Fact]
        public async Task StorageMonitor_WhenStorageUsageInvalid_LogsErrorMessage()
        {
            // Arrange
            string invalidStorageUsage = "Filesystem     1K-blocks     Used Available Use% Mounted on\r\n/dev/sda2       25623780 15600484   8696348  asdd% /\r\n";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(invalidStorageUsage);

            // Act
            await _storageMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[STORAGE] Parsing failed"))), Times.Once);
        }

        [Fact]
        public async Task StorageMonitor_WhenException_LogsErrorMessage()
        {
            // Arrange
            string exception = "Exception";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exception));

            // Act
            await _storageMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[STORAGE] Monitoring failed: Exception"))), Times.Once);
        }


    }
}
