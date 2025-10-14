using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set transition preview state for a mix effect
/// </summary>
[Command("CTPr")]
public class PreviewTransitionCommand : SerializedCommand
{
    private bool _preview;

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
    public PreviewTransitionCommand(int mixEffectId, AtemState currentState)
    {
        MixEffectId = mixEffectId;

        // If no video state or mix effect array exists, initialize with defaults
        if (!currentState.Video.MixEffects.TryGetValue(mixEffectId, out var mixEffect))
        {
            // Set default value and flag (like TypeScript pattern)
            Preview = false;
            return;
        }

        // Initialize from current state (direct field access = no flags set)
        _preview = mixEffect.TransitionPreview;
    }

    /// <summary>
    /// Whether transition preview is enabled
    /// </summary>
    public bool Preview
    {
        get => _preview;
        set
        {
            _preview = value;
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
        writer.Write((byte)(Preview ? 1 : 0));
        writer.Pad(2); // Padding to match 4-byte buffer size
        
        return memoryStream.ToArray();
    }
}