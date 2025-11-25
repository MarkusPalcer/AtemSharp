using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.State;

/// <summary>
/// Utility methods for working with ATEM state
/// </summary>
public static class AtemStateUtil
{
    public static T[] CreateArray<T>(int length) where T : ArrayItem, new()
    {
        if (length <= 0) return [];

        var result = Enumerable.Repeat(() => new T(), length).Select(x => x()).ToArray();
        foreach (var (value, id) in result.Select((x, i) => (x, i)))
        {
            value.SetId(id);
        }

        return result;
    }

    public static FairlightAudioState GetFairlight(this AtemState state)
        => state.Audio as FairlightAudioState ?? throw new InvalidOperationException("Fairlight audio state is not available");

    public static ClassicAudioState GetClassicAudio(this AtemState state)
        => state.Audio as ClassicAudioState ?? throw new InvalidOperationException("Classic audio state is not available");

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
