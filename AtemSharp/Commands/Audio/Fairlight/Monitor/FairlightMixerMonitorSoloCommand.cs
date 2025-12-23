using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight.Monitor;

/// <summary>
/// Used to set the solo properties of the fairlight mixer
/// </summary>
[Command("CFMS")]
[BufferSize(24)]
public partial class FairlightMixerMonitorSoloCommand(FairlightAudioState state) : SerializedCommand
{
    [SerializedField(1, 0)] private bool _solo = state.Solo.Solo;

    [SerializedField(8, 1)] private ushort _index = state.Solo.Index;

    // Flag field intentionally duplicated - see TypeScript implementation
    [SerializedField(16, 1)] private long _source = state.Solo.Source;
}
