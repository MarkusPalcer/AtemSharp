using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight;

[Command("AEBP")]
public class FairlightMixerSourceEqualizerBandUpdateCommand : IDeserializedCommand
{
    public ushort InputId { get; set; }
    public long SourceId { get; set; }
    public byte BandIndex { get; set; }

    public bool Enabled { get; set; }

    // TODO: Create enum from ATEM Mini ISO Pro behavior
    public uint[] SupportedShapes { get; set; } = [];

    public byte Shape { get; set; }

    public uint[] SupportedFrequencyRanges { get; set; } = [];

    // TODO: Create enum from ATEM Mini ISO Pro behavior
    public byte FrequencyRange { get; set; }

    public uint Frequency { get; set; }

    public double Gain { get; set; }

    public double QFacctor { get; set; }

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        return new FairlightMixerSourceEqualizerBandUpdateCommand
        {
            InputId = rawCommand.ReadUInt16BigEndian(0),
            SourceId = rawCommand.ReadInt64BigEndian(8),
            BandIndex = rawCommand.ReadUInt8(16),
            Enabled = rawCommand.ReadBoolean(17),
            SupportedShapes = AtemUtil.GetComponents(rawCommand.ReadUInt8(18)),
            Shape = rawCommand.ReadUInt8(19),
            SupportedFrequencyRanges = AtemUtil.GetComponents(rawCommand.ReadUInt8(20)),
            FrequencyRange = rawCommand.ReadUInt8(21),
            Frequency = rawCommand.ReadUInt32BigEndian(24),
            Gain = rawCommand.ReadInt32BigEndian(28) / 100.0,
            QFacctor = rawCommand.ReadInt16BigEndian(32) / 100.0,
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

        if (BandIndex >= source.Equalizer.Bands.Length)
        {
            throw new IndexOutOfRangeException($"Band Index {BandIndex} does not exist on Source ID {SourceId} on Input ID {InputId}");
        }
        var band = source.Equalizer.Bands[BandIndex];

        band.Enabled = Enabled;
        band.SupportedShapes = SupportedShapes;
        band.Shape = Shape;
        band.SupportedFrequencyRanges = SupportedFrequencyRanges;
        band.FrequencyRange = FrequencyRange;
        band.Frequency = Frequency;
        band.Gain = Gain;
        band.QFactor = QFacctor;
    }
}
