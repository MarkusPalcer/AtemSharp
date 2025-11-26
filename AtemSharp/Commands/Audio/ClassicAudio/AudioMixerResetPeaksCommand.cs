using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio.ClassicAudio;

/// <summary>
/// Command to reset audio mixer peak indicators
/// </summary>
[Command("RAMP")]
[BufferSize(8)]
public partial class AudioMixerResetPeaksCommand(ClassicAudioChannel channel) : SerializedCommand
{
    /// <summary>
    /// Reset all audio input peaks
    /// </summary>
    [SerializedField(1, 0)] private bool _all;

    [SerializedField(2, 1)] [NoProperty] private readonly ushort _input = channel.Id;

    /// <summary>
    /// Reset master audio peaks
    /// </summary>
    [SerializedField(4, 2)] private bool _master;

    /// <summary>
    /// Reset monitor audio peaks
    /// </summary>
    [SerializedField(5, 3)] private bool _monitor;
}
