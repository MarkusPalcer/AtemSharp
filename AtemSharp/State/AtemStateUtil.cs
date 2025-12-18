using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.State;

/// <summary>
/// Utility methods for working with ATEM state
/// </summary>
public static class AtemStateUtil
{
    public static FairlightAudioState GetFairlight(this AtemState state)
        => state.Audio as FairlightAudioState ?? throw new InvalidOperationException("Fairlight audio state is not available");

    public static ClassicAudioState GetClassicAudio(this AtemState state)
        => state.Audio as ClassicAudioState ?? throw new InvalidOperationException("Classic audio state is not available");
}
