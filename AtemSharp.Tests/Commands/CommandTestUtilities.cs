namespace AtemSharp.Tests.Commands;

public static class CommandTestUtilities
{
    /// <summary>
    /// Combine individual flag components into a single value
    /// </summary>
    /// <param name="values">Array of flag values to combine</param>
    /// <returns>Combined flag value</returns>
    public static T CombineComponents<T>(this T[] values) where T : Enum
    {
        var result = values.Aggregate(0, (current, value) => current | Convert.ToInt32(value));
        return (T)Enum.ToObject(typeof(T), result);
    }
}
