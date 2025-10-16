using AtemSharp.Enums;
using AtemSharp.Enums.Fairlight;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CFIP", ProtocolVersion.V8_1_1)]
public class FairlightMixerInputV8Command : SerializedCommand
{
    private FairlightInputConfiguration _activeConfiguration;
    private FairlightAnalogInputLevel _activeInputLevel;

    public FairlightInputConfiguration ActiveConfiguration
    {
        get => _activeConfiguration;
        set
        {
            _activeConfiguration = value;
            Flag |= 1 << 0;
        }
    }

    public FairlightAnalogInputLevel ActiveInputLevel
    {
        get => _activeInputLevel;
        set
        {
            _activeInputLevel = value;
            Flag |= 1 << 1;
        }
    }

    public ushort Index { get; private set; }

    public FairlightMixerInputV8Command(FairlightAudioInput input)
    {
        Init(input);
    }

    public FairlightMixerInputV8Command(AtemState state, ushort index)
    {
        Init(state.GetFairlight().Inputs[index]);
    }

    private void Init(FairlightAudioInput input)
    {
        Index = input.Id;
        ActiveConfiguration = input.Properties.ActiveConfiguration;
        ActiveInputLevel = input.Properties.ActiveInputLevel;
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[8];
        buffer.WriteUInt8((byte)Flag, 0);
        buffer.WriteUInt16BigEndian(Index, 2);
        buffer.WriteUInt8((byte)ActiveConfiguration, 4);
        buffer.WriteUInt8((byte)ActiveInputLevel, 5);
        return buffer;
    }
}
