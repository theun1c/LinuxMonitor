using Moq;
using ServerMonitoringAgent.Executors;
using ServerMonitoringAgent.Logging;
using ServerMonitoringAgent.Monitor.DockerProcess.Containers.Etcd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAgent.Tests.Monitor.DockerProcess.Containers.Etcd
{
    public class EtcdContainerMonitorTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<ILinuxExecutor> _executorMock;
        private readonly EtcdContainerMonitor _etcdConMonitor;

        public EtcdContainerMonitorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _executorMock = new Mock<ILinuxExecutor>();
            _etcdConMonitor = new EtcdContainerMonitor(_loggerMock.Object, _executorMock.Object);
        }

        [Fact]
        public async Task EtcdMonotor_WhenContainerAvailable_LogsInfoMessage()
        {
            // Arrange 
            string validOutput = "... ... ...    5 days ago   Up 5 days             etcd";
            _executorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _etcdConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("[ETCDCON] 1"))), Times.Once());
        }

        [Fact]
        public async Task EtcdMonotor_WhenContainerUnavailable_LogsWarnMessage()
        {
            // Arrange 
            string validOutput = " ";
            _executorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ReturnsAsync(validOutput);

            // Act
            await _etcdConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Warn(It.Is<string>(s => s.Contains("[ETCDCON] 0"))), Times.Once());
        }

        [Fact]
        public async Task EtcdMonotor_WhenException_LogsErrorMessage()
        {
            // Arrange 
            string exception = "ex message";
            _executorMock.Setup(x => x.ExecuteLinuxCommandAsync(It.IsAny<string>())).ThrowsAsync(new Exception(exception));

            // Act
            await _etcdConMonitor.MonitorAsync();

            // Assert
            _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("[ETCDCON] Monitoring failed: ex message"))), Times.Once());
        }
    }
}
