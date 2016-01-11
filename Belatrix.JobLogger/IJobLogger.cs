namespace Belatrix.JobLogger
{
    public interface IJobLogger
    {
        void LogMessage(string message, LogLevel logLevel);
    }
}