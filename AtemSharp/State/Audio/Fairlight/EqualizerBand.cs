using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public abstract class EqualizerBand : ItemWithId<int>
{
    internal override void SetId(int id) => Id = (byte)id;
    public byte Id { get; internal set; }
    public bool Enabled { get; internal set; }
    public byte[] SupportedShapes { get; internal set; } = [];
    public byte Shape { get; internal set; }
    public byte[] SupportedFrequencyRanges { get; internal set; } = [];
    public byte FrequencyRange { get; internal set; }
    public uint Frequency { get; internal set; }
    public double Gain { get; internal set; }
    public double QFactor { get; internal set; }
}
