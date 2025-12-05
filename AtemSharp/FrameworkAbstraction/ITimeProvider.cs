namespace AtemSharp.FrameworkAbstraction;

public interface ITimeProvider
{
    DateTime Now { get; }

    /// <summary>
    /// Schedules a delay that completes when virtual time advances past Now + delay.
    /// </summary>
    Task Delay(TimeSpan delay, CancellationToken token);
}
