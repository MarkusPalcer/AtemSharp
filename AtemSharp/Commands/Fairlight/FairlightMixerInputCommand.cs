using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CFIP")]
[BufferSize(8)]
public partial class FairlightMixerInputCommand : SerializedCommand
{
    [SerializedField(2)] private ushort _index;

    [SerializedField(4, 0)] private bool _rcaToXlrEnabled;

    [SerializedField(5, 1)] private FairlightInputConfiguration _activeConfiguration;

    public FairlightMixerInputCommand(FairlightAudioInput input)
    {
        _index = input.Id;
        RcaToXlrEnabled = input.RcaToXlrEnabled;
        ActiveConfiguration = input.ActiveConfiguration;
    }
}
