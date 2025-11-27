using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class Dynamics
{
    public double MakeUpGain { get; internal set; }
    public Expander Expander { get; } = new();
    public Compressor Compressor { get; } = new();
    public Limiter Limiter { get; } = new();
}
