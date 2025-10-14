using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to set the on-air state of a downstream keyer
/// </summary>
[Command("CDsL")]
public class DownstreamKeyOnAirCommand : SerializedCommand
{
    private bool _onAir;

    /// <summary>
    /// Downstream keyer index (0-based)
    /// </summary>
    public int DownstreamKeyerId { get; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="downstreamKeyerId">Downstream keyer index (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if downstream keyer not available</exception>
    public DownstreamKeyOnAirCommand(int downstreamKeyerId, AtemState currentState)
    {
        DownstreamKeyerId = downstreamKeyerId;

        if (downstreamKeyerId >= currentState.Video.DownstreamKeyers.Length)
        {
            throw new IndexOutOfRangeException("DownstreamKeyerId is out of range");
        }

        var dsk = currentState.Video.DownstreamKeyers[downstreamKeyerId];

        // Initialize from current state (direct field access = no flags set)
        _onAir = dsk.OnAir;
    }

    /// <summary>
    /// Whether the downstream keyer is on air
    /// </summary>
    public bool OnAir
    {
        get => _onAir;
        set
        {
            _onAir = value;
            Flag |= 1 << 0;  // Set flag for onAir property
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

        // Based on TypeScript serialization: downstreamKeyerId at byte 0, onAir at byte 1
        writer.Write((byte)DownstreamKeyerId);
        writer.WriteBoolean(OnAir);
        writer.Pad(2); // Pad to 4 bytes total

        return memoryStream.ToArray();
    }
}
