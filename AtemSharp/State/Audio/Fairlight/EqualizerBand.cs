using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public abstract class EqualizerBand
{
    public byte Id { get; internal init; }
    public bool Enabled { get; internal set; }
    public Shape[] SupportedShapes { get; internal set; } = [];
    public Shape Shape { get; internal set; }
    public FrequencyRange[] SupportedFrequencyRanges { get; internal set; } = [];
    public FrequencyRange FrequencyRange { get; internal set; }
    public uint Frequency { get; internal set; }
    public double Gain { get; internal set; }
    public double QFactor { get; internal set; }

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{GetType().Name} #{Id}";
}
