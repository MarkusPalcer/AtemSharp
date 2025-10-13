namespace AtemSharp.Tests.TestUtilities;

public static class Utilities
{
	private const double FloatingPointTolerance = 0.01;
	
	/// <summary>
	/// Wraps a Task&lt;T&gt; with a timeout to ensure test operations don't hang indefinitely.
	/// This extension method provides a safety mechanism for async test operations that might
	/// fail to complete due to deadlocks, network issues, or other blocking conditions.
	/// </summary>
	/// <typeparam name="T">The type of result returned by the task</typeparam>
	/// <param name="task">The task to execute with timeout protection</param>
	/// <returns>The result of the completed task</returns>
	/// <exception cref="AssertionException">Thrown when the task does not complete within the 5-second timeout period</exception>
	/// <example>
	/// <code>
	/// var result = await SomeAsyncOperation().WithTimeout();
	/// Assert.That(result, Is.EqualTo(expectedValue));
	/// </code>
	/// </example>
	public static async Task<T> WithTimeout<T>(this Task<T> task)
	{
		var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
		var result = await Task.WhenAny(task, timeoutTask);
		Assert.That(result, Is.Not.SameAs(timeoutTask), "Timeout expired before task completed");
		return await task;
	}
	
	/// <summary>
	/// Wraps a Task with a timeout to ensure test operations don't hang indefinitely.
	/// This extension method provides a safety mechanism for async test operations that might
	/// fail to complete due to deadlocks, network issues, or other blocking conditions.
	/// Use this overload for tasks that don't return a value (void async methods).
	/// </summary>
	/// <param name="task">The task to execute with timeout protection</param>
	/// <returns>A completed Task when the original task finishes successfully</returns>
	/// <exception cref="AssertionException">Thrown when the task does not complete within the 5-second timeout period</exception>
	/// <example>
	/// <code>
	/// await SomeAsyncVoidOperation().WithTimeout();
	/// // Test continues after successful completion
	/// </code>
	/// </example>
	public static async Task WithTimeout(this Task task)
	{
		var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
		var result = await Task.WhenAny(task, timeoutTask);
		Assert.That(result, Is.Not.SameAs(timeoutTask), "Timeout expired before task completed");
		
		await task;
	}

	/// <summary>
	/// Compares two float values for approximate equality by rounding to the specified number of decimal places.
	/// This is useful when test data has limited precision and exact floating-point comparison is not appropriate.
	/// Use this method when the ATEM binary protocol stores values with specific scaling factors:
	/// - 1 decimal place: Values scaled by 10 (e.g., clip/gain values stored as value * 10)
	/// - 2 decimal places: Values scaled by 100 (e.g., percentage values stored as percentage * 100)
	/// - Higher precision: Values with more complex scaling or conversion factors
	/// </summary>
	/// <param name="actual">The actual value from the command</param>
	/// <param name="expected">The expected value from test data</param>
	/// <param name="decimals">The number of decimal places to round to before comparison</param>
	/// <returns>True if the values are equal after rounding to the specified decimal places</returns>
	public static bool AreApproximatelyEqual(float actual, float expected, int decimals)
	{
		return AreApproximatelyEqual(Math.Round(actual, decimals), Math.Round(expected, decimals), 1.0/(float)Math.Pow(10, decimals+1));
	}

	/// <summary>
	/// Compares two double values for approximate equality by rounding to the specified number of decimal places.
	/// This is useful when test data has limited precision and exact floating-point comparison is not appropriate.
	/// Use this method when the ATEM binary protocol stores values with specific scaling factors:
	/// - 1 decimal place: Values scaled by 10 (e.g., clip/gain values stored as value * 10)
	/// - 2 decimal places: Values scaled by 100 (e.g., percentage values stored as percentage * 100)
	/// - Higher precision: Values with more complex scaling or conversion factors
	/// </summary>
	/// <param name="actual">The actual value from the command</param>
	/// <param name="expected">The expected value from test data</param>
	/// <param name="decimals">The number of decimal places to round to before comparison</param>
	/// <returns>True if the values are equal after rounding to the specified decimal places</returns>
	public static bool AreApproximatelyEqual(double actual, double expected, int decimals)
	{
		return AreApproximatelyEqual(Math.Round(actual, decimals), Math.Round(expected, decimals), 1.0/Math.Pow(10, decimals+1));
	}

	/// <summary>
	/// Compares two double values for approximate equality using a fixed tolerance.
	/// </summary>
	/// <param name="actual">The actual value from the command</param>
	/// <param name="expected">The expected value from test data</param>
	/// <param name="tolerance">How much difference between the two numbers is allowed before they are considered non-equal</param>
	/// <returns>True if the values are within the floating-point tolerance</returns>
	public static bool AreApproximatelyEqual(double actual, double expected, double tolerance = FloatingPointTolerance)
	{
		if (double.IsInfinity(expected) && double.IsInfinity(actual))
			return Math.Sign(expected) == Math.Sign(actual);
		
		return Math.Abs(actual - expected) <= tolerance;
	}
}