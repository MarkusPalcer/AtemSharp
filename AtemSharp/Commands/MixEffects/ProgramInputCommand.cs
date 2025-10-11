using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects;

/// <summary>
/// Command to set the program input source for a mix effect
/// </summary>
[Command("CPgI")]
public class ProgramInputCommand : SerializedCommand
{
    private int _source;

    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="mixEffectId">Mix effect index (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if mix effect not available</exception>
    public ProgramInputCommand(int mixEffectId, AtemState currentState)
    {
        MixEffectId = mixEffectId;

        // If no video state or mix effect array exists, initialize with defaults
        if (!currentState.Video.MixEffects.TryGetValue(mixEffectId, out var mixEffect))
        {
            // Set default value and flag (like TypeScript pattern)
            Source = 0;
            return;
        }

        // Initialize from current state (direct field access = no flags set)
        _source = mixEffect.ProgramInput;
    }

    /// <summary>
    /// Program input source number
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

        writer.Write((byte)MixEffectId);
        writer.Pad(1); 
        writer.WriteUInt16BigEndian((ushort)Source);
        
        return memoryStream.ToArray();
    }
}