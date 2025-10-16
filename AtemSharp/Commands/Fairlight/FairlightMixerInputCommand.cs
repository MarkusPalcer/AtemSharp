using AtemSharp.Enums;
using AtemSharp.Enums.Fairlight;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CFIP")]
public class FairlightMixerInputCommand : SerializedCommand
{
    private bool _rcaToXlrEnabled;
    private FairlightInputConfiguration _activeConfiguration;

    public FairlightMixerInputCommand(FairlightAudioInput input)
    {
        Init(input);
    }

    public FairlightMixerInputCommand(AtemState state, ushort index)
    {
        if (state.Audio is not FairlightAudioState audio)
        {
            throw new InvalidOperationException("FairlightMixer information is not available");
        }

        Init(audio.Inputs[index]);
    }

    private void Init(FairlightAudioInput input)
    {
        Index = input.Id;
        RcaToXlrEnabled = input.Properties.RcaToXlrEnabled;
        ActiveConfiguration = input.Properties.ActiveConfiguration;
    }

    public bool RcaToXlrEnabled
    {
        get => _rcaToXlrEnabled;
        set
        {
            _rcaToXlrEnabled = value;
            Flag |= 1 << 0;
        }
    }

    public FairlightInputConfiguration ActiveConfiguration
    {
        get => _activeConfiguration;
        set
        {
            _activeConfiguration = value;
            Flag |= 1 << 1;
        }
    }

    public ushort Index { get; private set; }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[8];
        buffer.WriteUInt8((byte)this.Flag, 0);
        buffer.WriteUInt16BigEndian(Index, 2);

        buffer.WriteBoolean(RcaToXlrEnabled, 4);
        buffer.WriteUInt8((byte)ActiveConfiguration, 5);

        return buffer;
    }
}
