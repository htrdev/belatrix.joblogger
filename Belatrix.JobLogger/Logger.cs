using System;
using System.Collections.Generic;
using System.Linq;

namespace Belatrix.JobLogger
{
    public class Logger
    {
        private static ICollection<LogLevel> _logLevelsAllowed = new List<LogLevel> { LogLevel.Message, LogLevel.Warning, LogLevel.Error };
        private static ICollection<LogType> _logTypesAllowed = new List<LogType> { LogType.Console, LogType.Database, LogType.TextFile };
        private static IJobLogger _textFileJobLogger = new TextFileJobLogger();
        private static IJobLogger _consoleJobLogger = new ConsoleJobLogger();
        private static IJobLogger _databaseJobLogger = new DatabaseJobLogger();

        public static void SetUpLogLevelsAllowed(params LogLevel[] logLevels)
        {
            _logLevelsAllowed.Clear();

            foreach (var logLevel in logLevels)
            {
                _logLevelsAllowed.Add(logLevel);    
            }
        }

        public static void SetUpLogTypesAllowed(params LogType[] logTypes)
        {
            _logTypesAllowed.Clear();

            foreach (var logType in logTypes)
            {
                _logTypesAllowed.Add(logType);
            }
        }

        public static void SetUpJobLoggers(IJobLogger textFileJobLogger
                                        , IJobLogger consoleJobLogger
                                        , IJobLogger databaseJobLogger)
        {
            if (textFileJobLogger != null)
            {
                _textFileJobLogger = textFileJobLogger;
            }

            if (consoleJobLogger != null)
            {
                _consoleJobLogger = consoleJobLogger;
            }

            if (databaseJobLogger != null)
            {
                _databaseJobLogger = databaseJobLogger;
            }
        }

        public static bool LogMessage(string message, LogLevel logLevel)
        {
            var messageLogged = false;

            if (_logLevelsAllowed.Count <= 0)
            {
                throw new IndexOutOfRangeException("Log levels");
            }

            if (_logTypesAllowed.Count <= 0)
            {
                throw new IndexOutOfRangeException("Log types");
            }

            if (!_logLevelsAllowed.Contains(logLevel))
            {
                return messageLogged;
            }

            if (_logTypesAllowed.Contains(LogType.TextFile))
            {
                _textFileJobLogger.LogMessage(message, logLevel);
                messageLogged = true;
            }

            if (_logTypesAllowed.Contains(LogType.Database))
            {
                _databaseJobLogger.LogMessage(message, logLevel);
                messageLogged = true;
            }

            if (_logTypesAllowed.Contains(LogType.Console))
            {
                _consoleJobLogger.LogMessage(message, logLevel);
                messageLogged = true;
            }

            return messageLogged;
        }
    }
}