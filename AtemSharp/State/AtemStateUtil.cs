using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.State;

/// <summary>
/// Utility methods for working with ATEM state
/// </summary>
public static class AtemStateUtil
{
    private static IEnumerable<T> CreateEnumerable<T>(int length) where T : ItemWithId<int>, new()
    {
        for (var i = 0; i < length; i++)
        {
            var value = new T();
            value.SetId(i);
            yield return value;
        }
    }

    public static T[] CreateArray<T>(int length) where T : ItemWithId<int>, new()
        => CreateEnumerable<T>(length).ToArray();

    public static void ExpandToFit<T>(this IList<T> self, uint id) where T : ItemWithId<int>, new()
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

    [Obsolete]
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

    public static TValue GetOrCreate<TValue>(this IDictionary<ushort, TValue> dict, ushort index)
        where TValue : ItemWithId<ushort>, new()
    {
        ArgumentNullException.ThrowIfNull(dict);

        if (dict.TryGetValue(index, out var value))
        {
            return value;
        }

        value = new TValue();
        value.SetId(index);
        dict[index] = value;

        return value;
    }

    public static TValue GetOrCreate<TValue>(this IDictionary<long, TValue> dict, long index)
        where TValue : ItemWithId<long>, new()
    {
        ArgumentNullException.ThrowIfNull(dict);

        if (dict.TryGetValue(index, out var value))
        {
            return value;
        }

        value = new TValue();
        value.SetId(index);
        dict[index] = value;

        return value;
    }

    public static TValue GetOrCreate<TValue>(this IDictionary<byte, TValue> dict, byte index)
        where TValue : ItemWithId<byte>, new()
    {
        ArgumentNullException.ThrowIfNull(dict);

        if (dict.TryGetValue(index, out var value))
        {
            return value;
        }

        value = new TValue();
        value.SetId(index);
        dict[index] = value;

        return value;
    }

    public static TValue GetOrCreate<TValue>(this IDictionary<uint, TValue> dict, uint index)
        where TValue : ItemWithId<uint>, new()
    {
        ArgumentNullException.ThrowIfNull(dict);

        if (dict.TryGetValue(index, out var value))
        {
            return value;
        }

        value = new TValue();
        value.SetId(index);
        dict[index] = value;

        return value;
    }
}
