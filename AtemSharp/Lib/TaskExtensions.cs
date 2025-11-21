using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AtemSharp.Lib;

public static class TaskExtensions
{
    public static async void FireAndForget(this Task task, [CallerArgumentExpression(nameof(task))] string? name = null)
    {
        try
        {
            await task;
        }
        catch (Exception e)
        {
            Debug.Print($"{name} threw exception: {e.Message}\n{e.StackTrace}");
        }
    }
}
