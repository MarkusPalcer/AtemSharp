using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command to update luma key settings for an upstream keyer
/// </summary>
[Command("CKLm")]
public class MixEffectKeyLumaCommand : SerializedCommand
{
    private bool _preMultiplied;
    private double _clip;
    private double _gain;
    private bool _invert;

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
    public MixEffectKeyLumaCommand(int mixEffectId, int keyerId, AtemState currentState)
    {
        MixEffectId = mixEffectId;
        KeyerId = keyerId;

        // If no video state or mix effect doesn't exist, initialize with defaults
        if (!currentState.Video.MixEffects.TryGetValue(mixEffectId, out var mixEffect) ||
            !mixEffect.UpstreamKeyers.TryGetValue(keyerId, out var keyer) ||
            keyer.LumaSettings == null)
        {
            // Set default values and flags (like TypeScript pattern)
            PreMultiplied = false;
            Clip = 0.0;
            Gain = 0.0;
            Invert = false;
            return;
        }

        // Initialize from current state (direct field access = no flags set)
        _preMultiplied = keyer.LumaSettings.PreMultiplied;
        _clip = keyer.LumaSettings.Clip;
        _gain = keyer.LumaSettings.Gain;
        _invert = keyer.LumaSettings.Invert;
    }

    /// <summary>
    /// Whether the key should be treated as premultiplied
    /// </summary>
    public bool PreMultiplied
    {
        get => _preMultiplied;
        set
        {
            _preMultiplied = value;
            Flag |= 1 << 0;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Clip threshold value (0-100)
    /// </summary>
    public double Clip
    {
        get => _clip;
        set
        {
            if (value < 0.0 || value > 100.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Clip must be between 0.0 and 100.0");
            }
            
            _clip = value;
            Flag |= 1 << 1;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Gain value for the luma key (0-100)
    /// </summary>
    public double Gain
    {
        get => _gain;
        set
        {
            if (value < 0.0 || value > 100.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Gain must be between 0.0 and 100.0");
            }
            
            _gain = value;
            Flag |= 1 << 2;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Whether to invert the luma key
    /// </summary>
    public bool Invert
    {
        get => _invert;
        set
        {
            _invert = value;
            Flag |= 1 << 3;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(12);
        using var writer = new BinaryWriter(memoryStream);

        writer.Write((byte)Flag);
        writer.Write((byte)MixEffectId);
        writer.Write((byte)KeyerId);
        writer.WriteBoolean(PreMultiplied);
        
        // Convert percentage values to 16-bit integers (multiply by 10 to match TypeScript implementation)
        writer.WriteUInt16BigEndian((ushort)(Clip * 10));
        writer.WriteUInt16BigEndian((ushort)(Gain * 10));
        
        writer.WriteBoolean(Invert);
        writer.Pad(3); // Padding to match 12-byte structure
        
        return memoryStream.ToArray();
    }
}