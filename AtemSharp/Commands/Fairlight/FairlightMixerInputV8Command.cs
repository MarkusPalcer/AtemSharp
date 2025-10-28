using AtemSharp.Enums;
using AtemSharp.Enums.Fairlight;
using AtemSharp.Helpers;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CFIP", ProtocolVersion.V8_1_1)]
[BufferSize(8)]
public partial class FairlightMixerInputV8Command(FairlightAudioInput input) : SerializedCommand
{
    [SerializedField(2)]
    [NoProperty]
    private ushort _index = input.Id;

    [SerializedField(4)]
    private FairlightInputConfiguration _activeConfiguration = input.Properties.ActiveConfiguration;

    [SerializedField(5)]
    private FairlightAnalogInputLevel _activeInputLevel = input.Properties.ActiveInputLevel;
}
