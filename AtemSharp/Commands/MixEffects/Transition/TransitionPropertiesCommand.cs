using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set transition properties for a mix effect
/// </summary>
[Command("CTTp")]
public class TransitionPropertiesCommand : SerializedCommand
{
    private TransitionStyle _nextStyle;
    private TransitionSelection _nextSelection;

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
    public TransitionPropertiesCommand(int mixEffectId, AtemState currentState)
    {
        MixEffectId = mixEffectId;

        // If no video state or mix effect array exists, initialize with defaults
        if (!currentState.Video.MixEffects.TryGetValue(mixEffectId, out var mixEffect) || 
            mixEffect.TransitionProperties == null)
        {
            // Set default values and flags (like TypeScript pattern)
            NextStyle = TransitionStyle.Mix;
            NextSelection = TransitionSelection.Background;
            return;
        }

        // Initialize from current state (direct field access = no flags set)
        _nextStyle = mixEffect.TransitionProperties.NextStyle;
        _nextSelection = mixEffect.TransitionProperties.NextSelection;
    }

    /// <summary>
    /// The style for the next transition
    /// </summary>
    public TransitionStyle NextStyle
    {
        get => _nextStyle;
        set
        {
            _nextStyle = value;
            Flag |= 1 << 0;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// The selection for the next transition
    /// </summary>
    public TransitionSelection NextSelection
    {
        get => _nextSelection;
        set
        {
            _nextSelection = value;
            Flag |= 1 << 1;  // Automatic flag setting!
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

        writer.Write((byte)Flag);
        writer.Write((byte)MixEffectId);
        writer.Write((byte)NextStyle);
        writer.Write((byte)NextSelection);
        
        return memoryStream.ToArray();
    }
}