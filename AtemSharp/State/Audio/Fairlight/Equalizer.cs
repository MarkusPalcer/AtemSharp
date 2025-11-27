using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public abstract class Equalizer
{
    public bool Enabled { get; internal set; }
    public double Gain { get; internal set; }
}
