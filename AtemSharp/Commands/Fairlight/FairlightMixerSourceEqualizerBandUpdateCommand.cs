using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight;

[Command("AEBP")]
public class FairlightMixerSourceEqualizerBandUpdateCommand : IDeserializedCommand
{
    public ushort InputId { get; set; }
    public long SourceId { get; set; }

    public BandUpdateParameters Parameters { get; } = new();

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        return new FairlightMixerSourceEqualizerBandUpdateCommand
        {
            InputId = rawCommand.ReadUInt16BigEndian(0),
            SourceId = rawCommand.ReadInt64BigEndian(8),
            Parameters =
            {
                BandIndex = rawCommand.ReadUInt8(16),
                Enabled = rawCommand.ReadBoolean(17),
                SupportedShapes = AtemUtil.GetComponents(rawCommand.ReadUInt8(18)),
                Shape = rawCommand.ReadUInt8(19),
                SupportedFrequencyRanges = AtemUtil.GetComponents(rawCommand.ReadUInt8(20)),
                FrequencyRange = rawCommand.ReadUInt8(21),
                Frequency = rawCommand.ReadUInt32BigEndian(24),
                Gain = rawCommand.ReadInt32BigEndian(28) / 100.0,
                QFactor = rawCommand.ReadInt16BigEndian(32) / 100.0,
            }
        };
    }

    public void ApplyToState(AtemState state)
    {
        var audio = state.GetFairlight();

        if (!audio.Inputs.TryGetValue(InputId, out var input))
        {
            throw new IndexOutOfRangeException($"Input ID {InputId} does not exist");
        }

        if (!input.Sources.TryGetValue(SourceId, out var source))
        {
            throw new IndexOutOfRangeException($"Source ID {SourceId} does not exist on Input ID {InputId}");
        }

        if (Parameters.BandIndex >= source.Equalizer.Bands.Length)
        {
            throw new IndexOutOfRangeException($"Band Index {Parameters.BandIndex} does not exist on Source ID {SourceId} on Input ID {InputId}");
        }
        var band = source.Equalizer.Bands[Parameters.BandIndex];

        Parameters.ApplyTo(band);
    }
}
