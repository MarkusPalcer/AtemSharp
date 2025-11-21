using System.Diagnostics;

namespace AtemSharp.Tests;

public static class Constants
{
    public static TimeSpan SendTimeout => Debugger.IsAttached ? Timeout.InfiniteTimeSpan : TimeSpan.FromSeconds(5);
}
