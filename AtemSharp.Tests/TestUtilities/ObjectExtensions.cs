namespace AtemSharp.Tests.TestUtilities;

public static class ObjectExtensions
{
    public static TOut As<TOut>(this object? input, string? message = null) where TOut : class
    {
        var result = input as TOut;
        Assert.That(result, Is.Not.Null, message ?? $"Object is not of type {typeof(TOut).Name}");
        return result!;
    }
}
