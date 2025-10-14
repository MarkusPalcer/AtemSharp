using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands;

/// <summary>
/// Update command for time configuration mode from the ATEM device
/// </summary>
[Command("TCCc", ProtocolVersion.V8_1_1)]
public class TimeConfigUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Time mode for the ATEM device
    /// </summary>
    public TimeMode Mode { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static TimeConfigUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new TimeConfigUpdateCommand
        {
            Mode = (TimeMode)rawCommand.ReadUInt8(0)
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update the state object
        state.Settings.TimeMode = Mode;
    }
}
