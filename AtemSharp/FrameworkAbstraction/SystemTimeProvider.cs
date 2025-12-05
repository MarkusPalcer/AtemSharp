using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.FrameworkAbstraction;

[ExcludeFromCodeCoverage(Justification="Testing this means testing the .NET framework")]
public class SystemTimeProvider : ITimeProvider
{
    public DateTime Now => DateTime.Now;

    public Task Delay(TimeSpan delay, CancellationToken token)
        => Task.Delay(delay, token);
}

