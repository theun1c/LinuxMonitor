namespace ServerMonitoringAgent.Logging
{
    /// <summary>
    /// ENG: interface for logger 
    /// RUS: интерфейс для логгирования
    /// </summary>
    public interface ILogger
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
    }
}
