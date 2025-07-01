using Moq;
using ServerMonitoringAgent.BashExecutor;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using ServerMonitoringAgent.Monitor.CPU;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Tests.Monitor.CPU
{
    public class CpuMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly CpuMonitor _cpuMonitor;
        private readonly Mock<ILinuxExecutor> _linuxExecutorMock;

        public CpuMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _linuxExecutorMock = new Mock<ILinuxExecutor>();
            _cpuMonitor = new CpuMonitor(_loggerMock.Object, _linuxExecutorMock.Object);
        }

        [Fact]
        public async Task CpuMonitor_WhenCpuOutputValid_LogsInfoMessage()
        {
            // Arrange
            string validCpuOutput = "procs -----------memory---------- ---swap-- -----io---- -system-- ------cpu-----\r\n r  b   swpd   free   buff  cache   si   so    bi    bo   in   cs us sy id wa st\r\n 1  0      0 2879856  55868 554016    0    0  7557  1261  906 1748 10 19 69  2  0\r\n 2  0      0 2879856  55868 554016    0    0     0     0  962 2070  0  3 97  0  0\r\n";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validCpuOutput);

            // Act
            await _cpuMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("[CPU] 3"))), Times.Once);

        }

        [Fact]
        public async Task CpuMonitor_WhenIncorrectData_LogsErrorMessage()
        {
            // Arrange
            string invalidCpuOutput = "S";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(invalidCpuOutput);
            
            // Act
            await _cpuMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[CPU] Parsing failed"))), Times.Once);
        }

        [Fact]
        public async Task CpuMonitor_WhenException_LogsErrorMessage()
        {
            // Arrange
            string exceptionMessage = "Smth failed";
            _linuxExecutorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            await _cpuMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[CPU] Monitoring failed: Smth failed"))), Times.Once);

        }
    }
}
