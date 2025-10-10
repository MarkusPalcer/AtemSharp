using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to set the cut source input for a downstream keyer
/// </summary>
[Command("CDsC")]
public class DownstreamKeyCutSourceCommand : SerializedCommand
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
    public DownstreamKeyCutSourceCommand(int downstreamKeyerId, AtemState currentState)
    {
        DownstreamKeyerId = downstreamKeyerId;

        // If no video state or downstream keyer array exists, initialize with defaults
        if (currentState.Video?.DownstreamKeyers == null || 
            downstreamKeyerId >= currentState.Video.DownstreamKeyers.Length ||
            currentState.Video.DownstreamKeyers[downstreamKeyerId] == null ||
            currentState.Video.DownstreamKeyers[downstreamKeyerId]!.Sources == null)
        {
            // Set default value and flag (like TypeScript pattern)
            Input = 0;
            return;
        }

        var dsk = currentState.Video.DownstreamKeyers[downstreamKeyerId]!;
        
        // Initialize from current state (direct field access = no flags set)
        _input = dsk.Sources!.CutSource;
    }

    /// <summary>
    /// Cut source input number
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