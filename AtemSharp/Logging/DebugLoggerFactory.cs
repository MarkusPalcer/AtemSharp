using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace AtemSharp.Logging;

public class DebugLoggerFactory : ILoggerFactory
{
    public ILogger<T> CreateLogger<T>() => new DebugLogger<T>();

    public ILogger CreateLogger(string categoryName) =>
        new DebugLogger<object>(categoryName);

    public void AddProvider(ILoggerProvider provider) {}
    public void Dispose() {}

    private class DebugLogger<T> : ILogger<T>
    {
        private readonly string _categoryName;

        public DebugLogger() : this(typeof(T).FullName!) {}

        public DebugLogger(string categoryName)
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
            Debug.WriteLine($"{_categoryName} [{logLevel}] {formatter(state, exception)}");
        }

        private class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new NullScope();
            public void Dispose() {}
        }
    }
}
