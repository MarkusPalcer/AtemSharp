using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to set the fill source input for a downstream keyer
/// </summary>
[Command("CDsF")]
public class DownstreamKeyFillSourceCommand : SerializedCommand
{
    private int _input;

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
    public DownstreamKeyFillSourceCommand(int downstreamKeyerId, AtemState currentState)
    {
        DownstreamKeyerId = downstreamKeyerId;

        if (downstreamKeyerId >= currentState.Video.DownstreamKeyers.Length)
        {
            throw new IndexOutOfRangeException("DownstreamKeyerId is out of range");
        }

        var dsk = currentState.Video.DownstreamKeyers[downstreamKeyerId];

        // Initialize from current state (direct field access = no flags set)
        _input = dsk.Sources!.FillSource;
    }

    /// <summary>
    /// Fill source input number
    /// </summary>
    public int Input
    {
        get => _input;
        set
        {
            _input = value;
            Flag |= 1 << 0;  // Automatic flag setting!
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

        writer.Write((byte)DownstreamKeyerId);
        writer.Pad(1);
        writer.WriteUInt16BigEndian((ushort)Input);

        return memoryStream.ToArray();
    }
}
