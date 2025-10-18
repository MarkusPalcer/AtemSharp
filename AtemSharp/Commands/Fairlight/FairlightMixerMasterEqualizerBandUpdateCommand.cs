using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight;

[Command("AMBP")]
public class FairlightMixerMasterEqualizerBandUpdateCommand : IDeserializedCommand
{
    public BandUpdateParameters Parameters { get; } = new();

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        return new FairlightMixerMasterEqualizerBandUpdateCommand
        {
            Parameters =
            {
                BandIndex = rawCommand.ReadUInt8(0),
                Enabled = rawCommand.ReadBoolean(1),
                SupportedShapes = AtemUtil.GetComponents(rawCommand.ReadUInt8(2)),
                Shape = rawCommand.ReadUInt8(3),
                SupportedFrequencyRanges = AtemUtil.GetComponents(rawCommand.ReadUInt8(4)),
                FrequencyRange = rawCommand.ReadUInt8(5),
                Frequency = rawCommand.ReadUInt32BigEndian(8),
                Gain = rawCommand.ReadInt32BigEndian(12) / 100.0,
                QFactor = rawCommand.ReadInt16BigEndian(16) / 100.0,
            }
        };
    }

    public void ApplyToState(AtemState state)
    {
        var equalizer = state.GetFairlight().Master.Equalizer;
        if (Parameters.BandIndex >= equalizer.Bands.Length)
        {
            throw new IndexOutOfRangeException($"Band Index {Parameters.BandIndex} does not exist on Master equalizer");
        }

        Parameters.ApplyTo(equalizer.Bands[Parameters.BandIndex]);
    }
}
