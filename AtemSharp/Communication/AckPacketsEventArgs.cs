using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Communication;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class AckPacketsEventArgs : EventArgs
{
    public required int[] PacketIds { get; init; }
}
