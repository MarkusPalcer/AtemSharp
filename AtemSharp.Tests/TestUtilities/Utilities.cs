namespace AtemSharp.Tests.TestUtilities;

public static class Utilities
{
	public static async Task<T> WithTimeout<T>(this Task<T> task)
	{
		var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
		var result = await Task.WhenAny(task, timeoutTask);
		Assert.That(result, Is.Not.SameAs(timeoutTask), "Timeout expired before task completed");
		return await task;
	}
	
	public static async Task WithTimeout(this Task task)
	{
		var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
		var result = await Task.WhenAny(task, timeoutTask);
		Assert.That(result, Is.Not.SameAs(timeoutTask), "Timeout expired before task completed");
		
		await task;
	}
}