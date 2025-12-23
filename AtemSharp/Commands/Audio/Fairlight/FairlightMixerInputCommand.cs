using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight;

/// <summary>
/// Used to configure an input of the fairlight mixer
/// </summary>
[Command("CFIP")]
[BufferSize(8)]
public partial class FairlightMixerInputCommand(FairlightAudioInput input) : SerializedCommand
{
    [SerializedField(2)] private ushort _index = input.Id;

    [SerializedField(4, 0)] private bool _rcaToXlrEnabled = input.RcaToXlrEnabled;

    [SerializedField(5, 1)] private FairlightInputConfiguration _activeConfiguration = input.ActiveConfiguration;
}
