namespace AtemSharp.Tests.TestUtilities;

using System;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

public class LoggerFactory : ILoggerFactory
{
    public void AddProvider(ILoggerProvider provider)
    {
        // Not used, but must exist
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new Logger(categoryName);
    }

    public void Dispose()
    {
    }

    private class Logger : ILogger
    {
        private readonly string _categoryName;

        public Logger(string categoryName)
        {
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) where TState: notnull => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);

            // You can choose where to write:
            // TestContext.Out.WriteLine(...)  -> Test output
            // TestContext.Progress.WriteLine(...) -> Progress window
            TestContext.Out.WriteLine(
                $"{_categoryName} [{logLevel}] {message}");

            if (exception != null)
            {
                TestContext.Out.WriteLine(exception.ToString());
            }
        }

        private class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new NullScope();
            public void Dispose() { }
        }
    }
}
