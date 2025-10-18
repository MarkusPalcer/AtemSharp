using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("FAMP")]
public class FairlightMixerMasterUpdateCommand : IDeserializedCommand
{
    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new FairlightMixerMasterUpdateCommand
        {
            BandCount = rawCommand.ReadUInt8(0),
            EqualizerEnabled = rawCommand.ReadBoolean(1),
            EqualizerGain = rawCommand.ReadInt32BigEndian(4) / 100.0,
            MakeUpGain = rawCommand.ReadInt32BigEndian(8) / 100.0,
            FaderGain = rawCommand.ReadInt32BigEndian(12) / 100.0,
            FollowFadeToBlack = rawCommand.ReadBoolean(16)
        };
    }

    public bool FollowFadeToBlack { get; set; }

    public double FaderGain { get; set; }

    public double MakeUpGain { get; set; }

    public double EqualizerGain { get; set; }

    public bool EqualizerEnabled { get; set; }

    public byte BandCount { get; set; }

    public void ApplyToState(AtemState state)
    {
        var fairlight = state.GetFairlight();
        fairlight.Master.FaderGain = FaderGain;
        fairlight.Master.Dynamics.MakeUpGain = MakeUpGain;
        fairlight.Master.Equalizer.Enabled = EqualizerEnabled;
        fairlight.Master.Equalizer.Gain = EqualizerGain;
        fairlight.Master.FollowFadeToBlack = FollowFadeToBlack;

        if (fairlight.Master.Equalizer.Bands.Length != BandCount)
        {
            fairlight.Master.Equalizer.Bands = AtemStateUtil.CreateArray<MasterEqualizerBand>(BandCount)
                                                            .ForEachWithIndex((band, index) => band.Index = (byte)index);
        }
    }
}
