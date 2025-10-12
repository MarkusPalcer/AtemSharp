using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set the transition mix rate for a mix effect
/// </summary>
[Command("CTMx")]
public class TransitionMixCommand : SerializedCommand
{
    private int _rate;

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
    public TransitionMixCommand(int mixEffectId, AtemState currentState)
    {
        MixEffectId = mixEffectId;

        // If no video state or mix effect array exists, initialize with defaults
        if (!currentState.Video.MixEffects.TryGetValue(mixEffectId, out var mixEffect) ||
            mixEffect.TransitionSettings?.Mix == null)
        {
            // Set default value and flag (like TypeScript pattern)
            Rate = 25;
            return;
        }

        // Initialize from current state (direct field access = no flags set)
        _rate = mixEffect.TransitionSettings.Mix.Rate;
    }

    /// <summary>
    /// The rate of the mix transition in frames
    /// </summary>
    public int Rate
    {
        get => _rate;
        set
        {
            _rate = value;
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
        writer.Write((byte)Rate);
        writer.Pad(2); // Skip 2 bytes padding
        
        return memoryStream.ToArray();
    }
}