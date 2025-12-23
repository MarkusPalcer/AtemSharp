using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio.ClassicAudio;

/// <summary>
/// Command to reset audio mixer peak indicators
/// </summary>
[Command("RAMP")]
[BufferSize(8)]
public partial class AudioMixerResetPeaksCommand(ClassicAudioChannel channel) : SerializedCommand
{
    [SerializedField(1, 0)] private bool _all;

    [SerializedField(2, 1)] [NoProperty] private readonly ushort _input = channel.Id;

    [SerializedField(4, 2)] private bool _master;

    [SerializedField(5, 3)] private bool _monitor;
}
