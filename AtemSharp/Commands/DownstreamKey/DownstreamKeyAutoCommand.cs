using AtemSharp.State.Info;
using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to trigger an auto transition on a downstream keyer
/// </summary>
[Command("DDsA")]
public class DownstreamKeyAutoCommand(DownstreamKeyer dsk) : SerializedCommand
{
    private bool _isTowardsOnAir = dsk.IsTowardsOnAir;

    /// <summary>
    /// Downstream keyer index (0-based)
    /// </summary>
    public int DownstreamKeyerId { get; } = dsk.Id;

    /// <summary>
    /// Direction of the auto transition (true = towards on air, false = towards off air)
    /// </summary>
    public bool IsTowardsOnAir
    {
        get => _isTowardsOnAir;
        set
        {
            _isTowardsOnAir = value;
            Flag |= 1 << 0; // MaskFlags.isTowardsOnAir = 1
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4);
        using var writer = new BinaryWriter(memoryStream);

        if (version >= ProtocolVersion.V8_0_1)
        {
            // Modern protocol format with flag
            writer.Write((byte)Flag);
            writer.Write((byte)DownstreamKeyerId);
            writer.WriteBoolean(IsTowardsOnAir);
            writer.Pad(1);
        }
        else
        {
            // Legacy protocol format without flag - downstreamKeyerId first
            writer.Write((byte)DownstreamKeyerId);
            writer.Pad(3);
        }

        return memoryStream.ToArray();
    }
}
