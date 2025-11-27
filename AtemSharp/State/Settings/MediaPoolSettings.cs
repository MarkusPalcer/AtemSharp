using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Settings;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MediaPoolSettings
{
    public ushort[] MaxFrames { get; internal set; } = [];
    public ushort UnassignedFrames { get; internal set; }
}
