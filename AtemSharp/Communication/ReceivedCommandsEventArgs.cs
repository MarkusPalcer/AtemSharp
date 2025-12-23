using System.Diagnostics.CodeAnalysis;
using AtemSharp.Commands;

namespace AtemSharp.Communication;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
internal class ReceivedCommandsEventArgs : EventArgs
{
    public required IDeserializedCommand[] Commands { get; init; }
}
