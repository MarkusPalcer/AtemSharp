namespace AtemSharp.Commands.Audio.Fairlight.Source;

/// <summary>
/// Used to reset the settings of an equalizer band of a fairlight source
/// </summary>
[Command("RICE")]
[BufferSize(20)]
public partial class FairlightMixerSourceEqualizerResetCommand(State.Audio.Fairlight.Source source) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private readonly ushort _inputId = source.InputId;
    [SerializedField(8)] [NoProperty] private readonly long _sourceId = source.Id;
    [SerializedField(16, 0)] private bool _equalizer;
    [SerializedField(17, 1)] private byte _band;
}
