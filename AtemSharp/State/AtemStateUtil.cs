using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.State;

/// <summary>
/// Utility methods for working with ATEM state
/// </summary>
public static class AtemStateUtil
{
    private static IEnumerable<T> CreateEnumerable<T>(int length) where T : ArrayItem, new()
    {
        for (var i = 0; i < length; i++)
        {
            var value = new T();
            value.SetId(i);
            yield return value;
        }
    }

    public static T[] CreateArray<T>(int length) where T : ArrayItem, new()
        => CreateEnumerable<T>(length).ToArray();

    public static void ExpandToFit<T>(this IList<T> self, uint id) where T : ArrayItem, new()
    {
        while (self.Count <= id)
        {
            var newItem = new T();
            newItem.SetId(self.Count);
            self.Add(newItem);
        }
    }

    public static FairlightAudioState GetFairlight(this AtemState state)
        => state.Audio as FairlightAudioState ?? throw new InvalidOperationException("Fairlight audio state is not available");

    public static ClassicAudioState GetClassicAudio(this AtemState state)
        => state.Audio as ClassicAudioState ?? throw new InvalidOperationException("Classic audio state is not available");

    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey index)
        where TValue : new() where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dict);

        if (dict.TryGetValue(index, out var value))
        {
            return value;
        }

        value = new TValue();
        dict[index] = value;
        return value;
    }
}
