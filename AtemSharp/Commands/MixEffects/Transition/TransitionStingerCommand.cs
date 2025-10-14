using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set stinger transition settings for a mix effect
/// </summary>
[Command("CTSt")]
public class TransitionStingerCommand : SerializedCommand
{
    private int _source;
    private bool _preMultipliedKey;
    private double _clip;
    private double _gain;
    private bool _invert;
    private int _preroll;
    private int _clipDuration;
    private int _triggerPoint;
    private int _mixRate;

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
    public TransitionStingerCommand(int mixEffectId, AtemState currentState)
    {
        MixEffectId = mixEffectId;

        // If no video state or mix effect array exists, initialize with defaults
        if (!currentState.Video.MixEffects.TryGetValue(mixEffectId, out var mixEffect) ||
            mixEffect.TransitionSettings?.Stinger == null)
        {
            // Set default values and flags (like TypeScript pattern)
            Source = 0;                // Default source
            PreMultipliedKey = false;  // Default pre-multiplied key
            Clip = 0;                  // Default clip
            Gain = 0;                  // Default gain
            Invert = false;            // Default invert
            Preroll = 0;               // Default preroll
            ClipDuration = 0;          // Default clip duration
            TriggerPoint = 0;          // Default trigger point
            MixRate = 0;               // Default mix rate
            return;
        }

        // Initialize from current state (direct field access = no flags set)
        _source = mixEffect.TransitionSettings.Stinger.Source;
        _preMultipliedKey = mixEffect.TransitionSettings.Stinger.PreMultipliedKey;
        _clip = mixEffect.TransitionSettings.Stinger.Clip;
        _gain = mixEffect.TransitionSettings.Stinger.Gain;
        _invert = mixEffect.TransitionSettings.Stinger.Invert;
        _preroll = mixEffect.TransitionSettings.Stinger.Preroll;
        _clipDuration = mixEffect.TransitionSettings.Stinger.ClipDuration;
        _triggerPoint = mixEffect.TransitionSettings.Stinger.TriggerPoint;
        _mixRate = mixEffect.TransitionSettings.Stinger.MixRate;
    }

    /// <summary>
    /// Source for the stinger transition
    /// </summary>
    public int Source
    {
        get => _source;
        set
        {
            _source = value;
            Flag |= 1 << 0;  // Automatic flag setting for source
        }
    }

    /// <summary>
    /// Whether the key is pre-multiplied
    /// </summary>
    public bool PreMultipliedKey
    {
        get => _preMultipliedKey;
        set
        {
            _preMultipliedKey = value;
            Flag |= 1 << 1;  // Automatic flag setting for preMultipliedKey
        }
    }

    /// <summary>
    /// Clip value for the stinger transition
    /// </summary>
    public double Clip
    {
        get => _clip;
        set
        {
            _clip = value;
            Flag |= 1 << 2;  // Automatic flag setting for clip
        }
    }

    /// <summary>
    /// Gain value for the stinger transition (0-100%)
    /// </summary>
    public double Gain
    {
        get => _gain;
        set
        {
            _gain = value;
            Flag |= 1 << 3;  // Automatic flag setting for gain
        }
    }

    /// <summary>
    /// Whether the stinger transition is inverted
    /// </summary>
    public bool Invert
    {
        get => _invert;
        set
        {
            _invert = value;
            Flag |= 1 << 4;  // Automatic flag setting for invert
        }
    }

    /// <summary>
    /// Preroll value for the stinger transition
    /// </summary>
    public int Preroll
    {
        get => _preroll;
        set
        {
            _preroll = value;
            Flag |= 1 << 5;  // Automatic flag setting for preroll
        }
    }

    /// <summary>
    /// Clip duration for the stinger transition
    /// </summary>
    public int ClipDuration
    {
        get => _clipDuration;
        set
        {
            _clipDuration = value;
            Flag |= 1 << 6;  // Automatic flag setting for clipDuration
        }
    }

    /// <summary>
    /// Trigger point for the stinger transition
    /// </summary>
    public int TriggerPoint
    {
        get => _triggerPoint;
        set
        {
            _triggerPoint = value;
            Flag |= 1 << 7;  // Automatic flag setting for triggerPoint
        }
    }

    /// <summary>
    /// Mix rate for the stinger transition
    /// </summary>
    public int MixRate
    {
        get => _mixRate;
        set
        {
            _mixRate = value;
            Flag |= 1 << 8;  // Automatic flag setting for mixRate
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(20);
        using var writer = new BinaryWriter(memoryStream);

        // Write flag as 16-bit big endian (matching TypeScript buffer.writeUInt16BE)
        writer.WriteUInt16BigEndian((ushort)Flag);
        
        // Write mix effect index and source
        writer.Write((byte)MixEffectId);             // Mix effect index
        writer.Write((byte)Source);                  // Source value
        writer.WriteBoolean(PreMultipliedKey); // Pre-multiplied key as byte
        writer.Pad(1);                               // 1 byte padding

        // Write clip and gain as 16-bit big endian values
        writer.WriteUInt16BigEndian((ushort)Math.Round(Clip * 10));  // Clip as value * 10
        writer.WriteUInt16BigEndian((ushort)Math.Round(Gain * 10));  // Gain as value * 10
        writer.WriteBoolean(Invert);        // Invert as byte
        writer.Pad(1);                               // 1 byte padding

        // Write remaining values as 16-bit big endian
        writer.WriteUInt16BigEndian((ushort)Preroll);      // Preroll
        writer.WriteUInt16BigEndian((ushort)ClipDuration); // Clip duration
        writer.WriteUInt16BigEndian((ushort)TriggerPoint); // Trigger point
        writer.WriteUInt16BigEndian((ushort)MixRate);      // Mix rate

        return memoryStream.ToArray();
    }
}