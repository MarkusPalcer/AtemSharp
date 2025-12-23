using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AtemSharp.Lib;

internal static class TaskExtensions
{
    [ExcludeFromCodeCoverage(Justification="Untestable")]
    public static async void FireAndForget(this Task task, [CallerArgumentExpression(nameof(task))] string? name = null)
    {
        try
        {
            await task;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"{name} threw exception\n{e.Message}\n{e.StackTrace}");
        }
    }
}
