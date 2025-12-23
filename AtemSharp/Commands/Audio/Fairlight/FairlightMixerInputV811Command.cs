using AtemSharp.State.Audio.Fairlight;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Audio.Fairlight;

/// <summary>
/// Used to configure an input of the fairlight mixer
/// </summary>
[Command("CFIP", ProtocolVersion.V8_1_1)]
[BufferSize(8)]
public partial class FairlightMixerInputV811Command(FairlightAudioInput input) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private readonly ushort _index = input.Id;

    [SerializedField(4)] private FairlightInputConfiguration _activeConfiguration = input.ActiveConfiguration;

    [SerializedField(5)] private FairlightAnalogInputLevel _activeInputLevel = input.ActiveInputLevel;
}
