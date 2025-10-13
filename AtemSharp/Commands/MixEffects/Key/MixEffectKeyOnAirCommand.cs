using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command to set the on-air state of an upstream keyer
/// </summary>
[Command("CKOn")]
public class MixEffectKeyOnAirCommand : SerializedCommand
{
    private bool _onAir;

    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public int KeyerId { get; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="mixEffectId">Mix effect index (0-based)</param>
    /// <param name="keyerId">Upstream keyer index (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if mix effect or keyer not available</exception>
    public MixEffectKeyOnAirCommand(int mixEffectId, int keyerId, AtemState currentState)
    {
        MixEffectId = mixEffectId;
        KeyerId = keyerId;

        // If no video state or mix effect doesn't exist, initialize with defaults
        if (!currentState.Video.MixEffects.TryGetValue(mixEffectId, out var mixEffect) ||
            !mixEffect.UpstreamKeyers.TryGetValue(keyerId, out var keyer))
        {
            // Set default value and flag (like TypeScript pattern)
            OnAir = false;
            return;
        }

        // Initialize from current state (direct field access = no flags set)
        _onAir = keyer.OnAir;
    }

    /// <summary>
    /// Whether the upstream keyer is on air
    /// </summary>
    public bool OnAir
    {
        get => _onAir;
        set
        {
            _onAir = value;
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
        writer.Write((byte)KeyerId);
        writer.WriteBoolean(OnAir);
        writer.Pad(1); // Padding to match 4-byte structure
        
        return memoryStream.ToArray();
    }
}