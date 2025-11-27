using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class SourceEqualizerBand : EqualizerBand
{
    public long SourceId { get; internal set; }

    public ushort InputId { get; internal set; }
}
