using AtemSharp.State.Audio.Fairlight;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Fairlight;

[Command("CFIP", ProtocolVersion.V8_1_1)]
[BufferSize(8)]
public partial class FairlightMixerInputV8Command(FairlightAudioInput input) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private ushort _index = input.Id;

    [SerializedField(4)] private FairlightInputConfiguration _activeConfiguration = input.ActiveConfiguration;

    [SerializedField(5)] private FairlightAnalogInputLevel _activeInputLevel = input.ActiveInputLevel;
}
