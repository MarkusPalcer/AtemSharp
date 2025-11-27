using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MasterEqualizer : Equalizer
{
    public MasterEqualizerBand[] Bands { get; internal set; } = [];
}
