using System;
using System.Configuration;
using System.IO;

namespace Belatrix.JobLogger
{
    public class TextFileJobLogger
        : IJobLogger
    {
        private readonly string _logFileDirectory;

        public TextFileJobLogger(string logFileDirectory)
        {
            _logFileDirectory = logFileDirectory;
        }

        public TextFileJobLogger()
        {
            _logFileDirectory = ConfigurationManager.AppSettings["LogFileDirectory"].ToString();
        }

        public void LogMessage(string message, LogLevel logLevel)
        {
            var logFilePath = _logFileDirectory + "LogFile" + DateTime.Now.ToShortDateString() + ".txt";
            string logFileContent = string.Empty;

            if (!File.Exists(logFilePath))
            {
                logFileContent = File.ReadAllText(logFilePath);
            }

            logFileContent += message;

            File.WriteAllText(logFilePath, logFileContent);
        }
    }
}