using Moq;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using ServerMonitoringAgent.Monitor.DockerProcess;
using ServerMonitoringAgent.Monitor.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Tests.Monitor.Memory
{
    public class MemoryMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly MemoryMonitor _memoryMonitor;
        private readonly Mock<ILinuxExecutor> _linuxExecutorMock;

        public MemoryMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _linuxExecutorMock = new Mock<ILinuxExecutor>();
            _memoryMonitor = new MemoryMonitor(_loggerMock.Object, _linuxExecutorMock.Object);
        }

        [Fact]
        public async Task MemoryMonitor_WhenOutputValid_LogsInfoMessage()
        {
            // Arrange
            string validOutput = "               total        used        free      shared  buff/cache   available\r\nMem:            3907         748        2698          10         691        3159\r\nSwap:           1023           0        1023\r\n";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _memoryMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("[MEMORY] 19"))), Times.Once);

        }

        [Fact]
        public async Task MemoryMonitor_WhenOutputInvalid_LogsErrorMessage()
        {
            // Arrange
            string invalidOutput = "Mem: asd asd";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(invalidOutput);

            // Act
            await _memoryMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[MEMORY] Converting failed"))), Times.Once);

        }

        [Fact]
        public async Task MemoryMonitor_WhenOutputNull_LogsErrorMessage()
        {
            // Arrange
            string nullOutput = "";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(nullOutput);

            // Act
            await _memoryMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[MEMORY] Parsing failed"))), Times.Once);

        }

        [Fact]
        public async Task MemoryMonitor_WhenException_LogsErrorMessage()
        {
            // Arrange
            string exceptionMessage = "ex message";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            await _memoryMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[MEMORY] Monitoring failed: ex message"))), Times.Once);

        }

    }
}
