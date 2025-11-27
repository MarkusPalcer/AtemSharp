using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace AtemSharp.Logging;

[ExcludeFromCodeCoverage]
public class DebugLoggerFactory : ILoggerFactory
{
    public ILogger<T> CreateLogger<T>() => new DebugLogger<T>();

    public ILogger CreateLogger(string categoryName) =>
        new DebugLogger<object>(categoryName);

    public void AddProvider(ILoggerProvider provider) {}
    public void Dispose() {}

    private class DebugLogger<T>(string categoryName) : ILogger<T>
    {
        private readonly string _categoryName = categoryName;

        public DebugLogger() : this(typeof(T).FullName!) {}

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => new NullScope();

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
            public void Dispose() {}
        }
    }
}
