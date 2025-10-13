using System.Text;
using AtemSharp.Enums;
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
    public int DownstreamKeyerId { get; set; }

    /// <summary>
    /// Downstream keyer properties
    /// </summary>
    public DownstreamKeyerProperties Properties { get; set; } = new();

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static DownstreamKeyPropertiesCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        var downstreamKeyerId = reader.ReadByte();
        var properties = new DownstreamKeyerProperties
        {
            Tie = reader.ReadBoolean(),
            Rate = reader.ReadByte(),
            
            PreMultiply = reader.ReadBoolean(),
            Clip = reader.ReadUInt16BigEndian() / 10.0, // Convert from fixed-point to double
            Gain = reader.ReadUInt16BigEndian() / 10.0, // Convert from fixed-point to double
            Invert = reader.ReadBoolean(),
            
            Mask = new DownstreamKeyerMask
            {
                Enabled = reader.ReadBoolean(),
                Top = reader.ReadInt16BigEndian() / 1000.0,    // Convert from thousandths
                Bottom = reader.ReadInt16BigEndian() / 1000.0, // Convert from thousandths
                Left = reader.ReadInt16BigEndian() / 1000.0,   // Convert from thousandths
                Right = reader.ReadInt16BigEndian() / 1000.0   // Convert from thousandths
            }
        };

        return new DownstreamKeyPropertiesCommand
        {
            DownstreamKeyerId = downstreamKeyerId,
            Properties = properties
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
    {
        // Validate downstream keyer index
        if (state.Info.Capabilities is null || DownstreamKeyerId >= state.Info.Capabilities.DownstreamKeyers)
        {
            throw new InvalidIdError("DownstreamKeyer", DownstreamKeyerId);
        }

        // Update the properties
        state.Video.DownstreamKeyers.GetOrCreate(DownstreamKeyerId).Properties = Properties;

        // Return the state path that was modified
        return [$"video.downstreamKeyers.{DownstreamKeyerId}.properties"];
    }
}