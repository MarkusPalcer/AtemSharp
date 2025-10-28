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
        return Enumerable.Repeat(() => new T(), length).Select(x => x()).ToArray();
    }

    public static FairlightAudioState GetFairlight(this AtemState state)
        => state.Audio as FairlightAudioState ?? throw new InvalidOperationException("Fairlight audio state is not available");

    public static ClassicAudioState GetClassicAudio(this AtemState state)
        => state.Audio as ClassicAudioState ?? throw new InvalidOperationException("Classic audio state is not available");

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

    /// <summary>
    /// Gets a MultiViewer from the state, creating it if it doesn't exist
    /// </summary>
    /// <param name="state">The ATEM state</param>
    /// <param name="index">The MultiViewer index</param>
    /// <returns>The MultiViewer instance</returns>
    public static MultiViewer GetMultiViewer(AtemState state, byte index)
    {
        var multiViewer = state.Settings.MultiViewers.GetOrCreate(index);
        multiViewer.Index = index; // Ensure index is set correctly
        return multiViewer;
    }

    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey index) where TValue : new() where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dict);

        if (dict.TryGetValue(index, out var value)) return value;
        value = new TValue();
        dict[index] = value;
        return value;
    }
}
