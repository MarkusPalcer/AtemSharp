namespace AtemSharp.Tests.Commands;

public static class CommandTestUtilities
{
    /// <summary>
    /// Combine individual flag components into a single value
    /// </summary>
    /// <param name="values">Array of flag values to combine</param>
    /// <returns>Combined flag value</returns>
    public static T CombineComponents<T>(T[] values) where T : Enum
    {
        int result = 0;
        foreach (var value in values)
        {
            result |= Convert.ToInt32(value);
        }
        return (T)Enum.ToObject(typeof(T), result);
    }
}
