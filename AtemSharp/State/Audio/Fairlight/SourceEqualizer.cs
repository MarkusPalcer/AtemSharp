using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class SourceEqualizer : Equalizer
{
    public SourceEqualizerBand[] Bands { get; internal set; } = [];
}
