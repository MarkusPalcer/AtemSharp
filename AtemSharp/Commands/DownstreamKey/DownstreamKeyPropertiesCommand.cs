using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command received from ATEM device containing downstream keyer properties
/// </summary>
[Command("DskP")]
public class DownstreamKeyPropertiesCommand : IDeserializedCommand
{
    /// <summary>
    /// Downstream keyer index (0-based)
    /// </summary>
    public int DownstreamKeyerId { get; init; }

    /// <summary>
    /// Downstream keyer properties
    /// </summary>
    public DownstreamKeyerProperties Properties { get; init; } = new();

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static DownstreamKeyPropertiesCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new DownstreamKeyPropertiesCommand
        {
            DownstreamKeyerId = rawCommand.ReadUInt8(0),
            Properties = new DownstreamKeyerProperties
            {
                Tie = rawCommand.ReadBoolean(1),
                Rate = rawCommand.ReadUInt8(2),

                PreMultiply = rawCommand.ReadBoolean(3),
                Clip = rawCommand.ReadUInt16BigEndian(4) / 10.0, // Convert from fixed-point to double
                Gain = rawCommand.ReadUInt16BigEndian(6) / 10.0, // Convert from fixed-point to double
                Invert = rawCommand.ReadBoolean(8),

                Mask = new DownstreamKeyerMask
                {
                    Enabled = rawCommand.ReadBoolean(9),
                    Top = rawCommand.ReadInt16BigEndian(10) / 1000.0,    // Convert from thousandths
                    Bottom = rawCommand.ReadInt16BigEndian(12) / 1000.0, // Convert from thousandths
                    Left = rawCommand.ReadInt16BigEndian(14) / 1000.0,   // Convert from thousandths
                    Right = rawCommand.ReadInt16BigEndian(16) / 1000.0   // Convert from thousandths
                }
            }
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update the properties
        state.Video.DownstreamKeyers[DownstreamKeyerId].Properties = Properties;
    }
}
