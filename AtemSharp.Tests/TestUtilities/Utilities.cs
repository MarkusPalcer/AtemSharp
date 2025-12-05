using System.Runtime.CompilerServices;

namespace AtemSharp.Tests.TestUtilities;

public static class Utilities
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(1);

	/// <summary>
	/// Wraps a Task&lt;T&gt; with a timeout to ensure test operations don't hang indefinitely.
	/// This extension method provides a safety mechanism for async test operations that might
	/// fail to complete due to deadlocks, network issues, or other blocking conditions.
	/// </summary>
	/// <example>
	/// <code>
	/// var result = await SomeAsyncOperation().WithTimeout();
	/// Assert.That(result, Is.EqualTo(expectedValue));
	/// </code>
	/// </example>
	public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan? timeout = null, [CallerArgumentExpression(nameof(task))] string? argumentName = null)
	{
		var timeoutTask = Task.Delay(timeout ?? DefaultTimeout);
		var result = await Task.WhenAny(task, timeoutTask);
		Assert.That(result, Is.Not.SameAs(timeoutTask), $"{argumentName} timed out");
		return await task;
	}

	/// <summary>
	/// Wraps a Task with a timeout to ensure test operations don't hang indefinitely.
	/// This extension method provides a safety mechanism for async test operations that might
	/// fail to complete due to deadlocks, network issues, or other blocking conditions.
	/// Use this overload for tasks that don't return a value (void async methods).
	/// </summary>
	/// <exception cref="AssertionException">Thrown when the task does not complete within the 5-second timeout period</exception>
	/// <example>
	/// <code>
	/// await SomeAsyncVoidOperation().WithTimeout();
	/// // Test continues after successful completion
	/// </code>
	/// </example>
	public static async Task WithTimeout(this Task task, TimeSpan? timeout = null, [CallerArgumentExpression(nameof(task))] string? argumentName = null)
	{
		var timeoutTask = Task.Delay(timeout ?? DefaultTimeout);
		var result = await Task.WhenAny(task, timeoutTask);
		Assert.That(result, Is.Not.SameAs(timeoutTask), $"{argumentName} timed out");

		await task;
	}

    public static async Task TimesOut(this Task task, TimeSpan? timeout = null, [CallerArgumentExpression(nameof(task))] string? argumentName = null)
    {
        var timeoutTask = Task.Delay(timeout ?? TimeSpan.FromMilliseconds(10));
        var result = await Task.WhenAny(task, timeoutTask);
        Assert.That(result, Is.SameAs(timeoutTask), $"{argumentName} should time out, but didn't");
    }
}
