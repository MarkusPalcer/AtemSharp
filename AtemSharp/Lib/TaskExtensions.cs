using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace AtemSharp.Lib;

public static class TaskExtensions
{
    public static async void FireAndForget(this Task task, ILogger logger, [CallerArgumentExpression(nameof(task))] string? name = null)
    {
        try
        {
            await task;
        }
        catch (Exception e)
        {
            logger.LogError(e, "{TaskExpression} threw exception", name);
        }
    }
}
