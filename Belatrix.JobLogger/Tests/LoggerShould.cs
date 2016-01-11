using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using Moq;

namespace Belatrix.JobLogger.Tests
{
    [TestFixture]
    public class LoggerShould
    {
        private Mock<IJobLogger> _textFileJobLoggerMock;
        private Mock<IJobLogger> _consoleJobLoggerMock;
        private Mock<IJobLogger> _databaseJobLoggerMock;

        [Test]
        public void ReturnFalseIfLogLevelIsNotAllowed()
        {
            Logger.SetUpLogLevelsAllowed(LogLevel.Message, LogLevel.Warning);
            var message = "Random error message";
            var expectedResult = false;

            var result = Logger.LogMessage(message, LogLevel.Error);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void LogMessageIfAllLogLevelsAreAllowedAndLogTypeIsTextFile()
        {
            Logger.SetUpLogLevelsAllowed(LogLevel.Message, LogLevel.Warning, LogLevel.Error);
            Logger.SetUpLogTypesAllowed(LogType.TextFile);
            var message = "Random error message";
            var expectedResult = true;
            SetUptJobLoggersMock();

            var result = Logger.LogMessage(message, LogLevel.Error);

            _textFileJobLoggerMock.Verify(m => m.LogMessage(message, LogLevel.Error), Times.Once());
            _consoleJobLoggerMock.Verify(m => m.LogMessage(message, LogLevel.Error), Times.Never());
            _databaseJobLoggerMock.Verify(m => m.LogMessage(message, LogLevel.Error), Times.Never());
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void LogMessageIfAllLogLevelsAreAllowedAndLogTypeIsConsole()
        {
            Logger.SetUpLogLevelsAllowed(LogLevel.Message, LogLevel.Warning, LogLevel.Error);
            Logger.SetUpLogTypesAllowed(LogType.Console);
            var message = "Random error message";
            var expectedResult = true;
            SetUptJobLoggersMock();

            var result = Logger.LogMessage(message, LogLevel.Error);

            _consoleJobLoggerMock.Verify(m => m.LogMessage(message, LogLevel.Error), Times.Once());
            _textFileJobLoggerMock.Verify(m => m.LogMessage(message, LogLevel.Error), Times.Never());
            _databaseJobLoggerMock.Verify(m => m.LogMessage(message, LogLevel.Error), Times.Never());
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void LogMessageIfAllLogLevelsAreAllowedAndLogTypeIsSqlServerDatabase()
        {
            Logger.SetUpLogLevelsAllowed(LogLevel.Message, LogLevel.Warning, LogLevel.Error);
            Logger.SetUpLogTypesAllowed(LogType.Database);
            var message = "Random error message";
            var expectedResult = true;
            SetUptJobLoggersMock();

            var result = Logger.LogMessage(message, LogLevel.Error);

            _consoleJobLoggerMock.Verify(m => m.LogMessage(message, LogLevel.Error), Times.Never());
            _textFileJobLoggerMock.Verify(m => m.LogMessage(message, LogLevel.Error), Times.Never());
            _databaseJobLoggerMock.Verify(m => m.LogMessage(message, LogLevel.Error), Times.Once());
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void LogMessageIfAllLogLevelsAreAllowedAndAllLogTypesAreAllowed()
        {
            Logger.SetUpLogLevelsAllowed(LogLevel.Message, LogLevel.Warning, LogLevel.Error);
            Logger.SetUpLogTypesAllowed(LogType.Database, LogType.Console, LogType.TextFile);
            var message = "Random error message";
            var expectedResult = true;
            SetUptJobLoggersMock();

            var result = Logger.LogMessage(message, LogLevel.Error);

            _consoleJobLoggerMock.Verify(m => m.LogMessage(message, LogLevel.Error), Times.Once());
            _textFileJobLoggerMock.Verify(m => m.LogMessage(message, LogLevel.Error), Times.Once());
            _databaseJobLoggerMock.Verify(m => m.LogMessage(message, LogLevel.Error), Times.Once());
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ThrowErrorIfLogLevelsAreEmpty()
        {
            Logger.SetUpLogLevelsAllowed();
            Logger.SetUpLogTypesAllowed(LogType.Database, LogType.Console, LogType.TextFile);
            var message = "Random error message";
            
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                Logger.LogMessage(message, LogLevel.Error);
            });
        }

        [Test]
        public void ThrowErrorIfLogTypesAreEmpty()
        {
            Logger.SetUpLogLevelsAllowed(LogLevel.Error, LogLevel.Warning, LogLevel.Message);
            Logger.SetUpLogTypesAllowed();
            var message = "Random error message";

            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                Logger.LogMessage(message, LogLevel.Error);
            });
        }

        private void SetUptJobLoggersMock()
        {
            _textFileJobLoggerMock = new Mock<IJobLogger>();
            _consoleJobLoggerMock = new Mock<IJobLogger>();
            _databaseJobLoggerMock = new Mock<IJobLogger>();

            Logger.SetUpJobLoggers(_textFileJobLoggerMock.Object, _consoleJobLoggerMock.Object, _databaseJobLoggerMock.Object);
        }
    }
}
