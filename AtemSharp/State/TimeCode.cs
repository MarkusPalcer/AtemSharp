using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class TimeCode
{
    public byte Hours { get; internal set; }
    public byte Minutes { get; internal set; }
    public byte Seconds { get; internal set; }
    public byte Frames { get; internal set; }

    public bool IsDropFrame { get; internal set; }
}
