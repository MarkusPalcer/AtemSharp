using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set dip transition settings for a mix effect
/// </summary>
[Command("CTDp")]
public class TransitionDipCommand : SerializedCommand
{
    private int _rate;
    private int _input;

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
    public TransitionDipCommand(int mixEffectId, AtemState currentState)
    {
        MixEffectId = mixEffectId;

        // If no video state or mix effect array exists, initialize with defaults
        if (!currentState.Video.MixEffects.TryGetValue(mixEffectId, out var mixEffect) ||
            mixEffect.TransitionSettings?.Dip == null)
        {
            // Set default values and flags (like TypeScript pattern)
            Rate = 25;  // Default rate
            Input = 0;  // Default input
            return;
        }

        // Initialize from current state (direct field access = no flags set)
        _rate = mixEffect.TransitionSettings.Dip.Rate;
        _input = mixEffect.TransitionSettings.Dip.Input;
    }

    /// <summary>
    /// Rate of the dip transition in frames
    /// </summary>
    public int Rate
    {
        get => _rate;
        set
        {
            _rate = value;
            Flag |= 1 << 0;  // Automatic flag setting for rate
        }
    }

    /// <summary>
    /// Input source for the dip transition
    /// </summary>
    public int Input
    {
        get => _input;
        set
        {
            _input = value;
            Flag |= 1 << 1;  // Automatic flag setting for input
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(8);
        using var writer = new BinaryWriter(memoryStream);

        writer.Write((byte)Flag);         // Flag as single byte
        writer.Write((byte)MixEffectId);  // Mix effect index
        writer.Write((byte)Rate);         // Rate value
        writer.Pad(1);                    // 1 byte padding
        writer.WriteUInt16BigEndian((ushort)Input); // Input as 16-bit big endian
        writer.Pad(2);                    // 2 bytes padding to reach 8 bytes total

        return memoryStream.ToArray();
    }
}