namespace ServerMonitoringAgent.Monitor
{
    /// <summary>
    /// ENG: interface of the monitoring class
    /// RUS: интерфейс класса мониторинга
    /// </summary>
    public interface IMonitor
    {
        Task MonitorAsync();
    }
}
