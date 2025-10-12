using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set the transition handle position for a mix effect
/// </summary>
[Command("CTPs")]
public class TransitionPositionCommand : SerializedCommand
{
    private double _handlePosition;

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
    public TransitionPositionCommand(int mixEffectId, AtemState currentState)
    {
        MixEffectId = mixEffectId;

        // If no video state or mix effect array exists, initialize with defaults
        if (!currentState.Video.MixEffects.TryGetValue(mixEffectId, out var mixEffect))
        {
            // Set default value and flag (like TypeScript pattern)
            HandlePosition = 0.0;
            return;
        }

        // Initialize from current state (direct field access = no flags set)
        _handlePosition = mixEffect.TransitionPosition.HandlePosition;
    }

    /// <summary>
    /// The position of the transition handle (0.0 to 1.0, where 1.0 = 100%)
    /// </summary>
    public double HandlePosition
    {
        get => _handlePosition;
        set
        {
            _handlePosition = value;
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
        writer.Pad(1); // Skip 1 byte
        
        // Convert from double (0.0-1.0) to ushort (0-10000) for transmission
        // Use Math.Round to match TypeScript behavior
        var handlePositionValue = (ushort)Math.Round(HandlePosition * 10000);
        writer.WriteUInt16BigEndian(handlePositionValue);
        
        return memoryStream.ToArray();
    }
}