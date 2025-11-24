namespace AtemSharp.Commands.Fairlight.Source;

[Command("RICE")]
[BufferSize(20)]
public partial class FairlightMixerSourceEqualizerResetCommand(State.Audio.Fairlight.Source source) : SerializedCommand
{
    [SerializedField(2)]
    [NoProperty]
    private readonly ushort _inputId = source.InputId;

    [SerializedField(8)]
    [NoProperty]
    private readonly long _sourceId = source.Id;

    [SerializedField(16,0)] private bool _equalizer;
    [SerializedField(17, 1)] private byte _band;
}
