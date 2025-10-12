using AtemSharp.Enums;
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
    public TimeMode Mode { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static TimeConfigUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mode = (TimeMode)reader.ReadByte();

        return new TimeConfigUpdateCommand
        {
            Mode = mode
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
    {
        // Update the state object
        state.Settings.TimeMode = Mode;
        
        // Return the state path that was modified for change tracking
        return ["settings.timeMode"];
    }
}