using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class Limiter
{
    public bool Enabled { get; internal set; }
    public double Threshold { get; internal set; }
    public double Attack { get; internal set; }
    public double Hold { get; internal set; }
    public double Release { get; internal set; }
}
