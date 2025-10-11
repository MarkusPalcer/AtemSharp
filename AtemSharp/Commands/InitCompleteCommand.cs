using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands;

/// <summary>
/// Command indicating that the ATEM has completed sending initial state information
/// </summary>
[Command("InCm")]
public class InitCompleteCommand : IDeserializedCommand
{
    public static InitCompleteCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        // This command has no data payload - it's just a marker
        return new InitCompleteCommand();
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
    {
        // This command doesn't modify state, but signals that initialization is complete
        // Return "info" to indicate that the general device information section was affected
        return ["info"];
    }
}