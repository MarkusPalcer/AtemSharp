using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.State;

/// <summary>
/// Utility methods for working with ATEM state
/// </summary>
public static class AtemStateUtil
{
    public static T[] CreateArray<T>(int length) where T : new()
    {
        if (length <= 0) return [];

        // TODO: Add ID to items here
        return Enumerable.Repeat(() => new T(), length).Select(x => x()).ToArray();
    }

    public static FairlightAudioState GetFairlight(this AtemState state)
        => state.Audio as FairlightAudioState ?? throw new InvalidOperationException("Fairlight audio state is not available");

    public static ClassicAudioState GetClassicAudio(this AtemState state)
        => state.Audio as ClassicAudioState ?? throw new InvalidOperationException("Classic audio state is not available");

    // TODO: remove when ID is added in CreateArray
    public static T[] ForEachWithIndex<T>(this T[] source, Action<T, int> action)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (action == null) throw new ArgumentNullException(nameof(action));

        int index = 0;
        foreach (var item in source)
        {
            action(item, index);
            index++;
        }

        return source;
    }

    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey index)
        where TValue : new() where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dict);

        if (dict.TryGetValue(index, out var value)) return value;
        value = new TValue();
        dict[index] = value;
        return value;
    }
}
