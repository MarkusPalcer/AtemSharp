using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands;

/// <summary>
/// Command to set the source for an auxiliary output
/// </summary>
[Command("CAuS")]
public class AuxSourceCommand : SerializedCommand
{
    private int _source;

    /// <summary>
    /// Auxiliary output index (0-based)
    /// </summary>
    public int AuxBus { get; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="auxBus">Auxiliary output index (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if auxiliary output not available</exception>
    public AuxSourceCommand(int auxBus, AtemState currentState)
    {
        AuxBus = auxBus;

        // If no video state or auxiliaries array exists, initialize with defaults
        if (currentState.Video?.Auxiliaries == null || 
            auxBus >= currentState.Video.Auxiliaries.Length ||
            currentState.Video.Auxiliaries[auxBus] == null)
        {
            // Set default value and flag (like TypeScript pattern)
            Source = 0;
            return;
        }

        var auxSource = currentState.Video.Auxiliaries[auxBus]!;
        
        // Initialize from current state (direct field access = no flags set)
        _source = auxSource.Value;
    }

    /// <summary>
    /// Source input number for the auxiliary output
    /// </summary>
    public int Source
    {
        get => _source;
        set
        {
            _source = value;
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

        writer.Write((byte)0x01);  // Flag byte
        writer.Write((byte)AuxBus);
        writer.WriteUInt16BigEndian((ushort)Source);
        
        return memoryStream.ToArray();
    }
}