using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command to update advanced chroma key sample settings for an upstream keyer
/// </summary>
[Command("CACC")]
public class MixEffectKeyAdvancedChromaSampleCommand : SerializedCommand
{
    private bool _enableCursor;
    private bool _preview;
    private double _cursorX;
    private double _cursorY;
    private double _cursorSize;
    private double _sampledY;
    private double _sampledCb;
    private double _sampledCr;

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
    public MixEffectKeyAdvancedChromaSampleCommand(int mixEffectId, int keyerId, AtemState currentState)
    {
        MixEffectId = mixEffectId;
        KeyerId = keyerId;

        // If no video state or mix effect doesn't exist, initialize with defaults
        if (!currentState.Video.MixEffects.TryGetValue(mixEffectId, out var mixEffect) ||
            !mixEffect.UpstreamKeyers.TryGetValue(keyerId, out var keyer) ||
            keyer.AdvancedChromaSettings?.Sample == null)
        {
            // Set default values and flags (like TypeScript pattern)
            EnableCursor = false;
            Preview = false;
            CursorX = 0.0;
            CursorY = 0.0;
            CursorSize = 0.0;
            SampledY = 0.0;
            SampledCb = 0.0;
            SampledCr = 0.0;
            return;
        }

        var sample = keyer.AdvancedChromaSettings.Sample;
        
        // Initialize from current state (direct field access = no flags set)
        _enableCursor = sample.EnableCursor;
        _preview = sample.Preview;
        _cursorX = sample.CursorX;
        _cursorY = sample.CursorY;
        _cursorSize = sample.CursorSize;
        _sampledY = sample.SampledY;
        _sampledCb = sample.SampledCb;
        _sampledCr = sample.SampledCr;
    }

    /// <summary>
    /// Whether the cursor is enabled
    /// </summary>
    public bool EnableCursor
    {
        get => _enableCursor;
        set
        {
            _enableCursor = value;
            Flag |= 1 << 0;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Whether to show preview
    /// </summary>
    public bool Preview
    {
        get => _preview;
        set
        {
            _preview = value;
            Flag |= 1 << 1;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Cursor X position
    /// </summary>
    public double CursorX
    {
        get => _cursorX;
        set
        {
            _cursorX = value;
            Flag |= 1 << 2;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Cursor Y position
    /// </summary>
    public double CursorY
    {
        get => _cursorY;
        set
        {
            _cursorY = value;
            Flag |= 1 << 3;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Cursor size
    /// </summary>
    public double CursorSize
    {
        get => _cursorSize;
        set
        {
            _cursorSize = value;
            Flag |= 1 << 4;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Sampled Y (luminance) value
    /// </summary>
    public double SampledY
    {
        get => _sampledY;
        set
        {
            _sampledY = value;
            Flag |= 1 << 5;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Sampled Cb (blue-difference chroma) value
    /// </summary>
    public double SampledCb
    {
        get => _sampledCb;
        set
        {
            _sampledCb = value;
            Flag |= 1 << 6;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Sampled Cr (red-difference chroma) value
    /// </summary>
    public double SampledCr
    {
        get => _sampledCr;
        set
        {
            _sampledCr = value;
            Flag |= 1 << 7;  // Automatic flag setting!
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

        // Write flag and identifiers
        writer.Write((byte)Flag);
        writer.Write((byte)MixEffectId);
        writer.Write((byte)KeyerId);

        // Write boolean values
        writer.WriteBoolean(EnableCursor);
        writer.WriteBoolean(Preview);
        writer.Pad(1); // Padding byte (TypeScript writes to offset 6)

        // Write position values (scaled by 1000 to match TypeScript implementation)
        writer.WriteInt16BigEndian((short)Math.Round(CursorX * 1000));
        writer.WriteInt16BigEndian((short)Math.Round(CursorY * 1000));
        
        // Write cursor size (scaled by 100 to match TypeScript implementation)
        writer.WriteUInt16BigEndian((ushort)Math.Round(CursorSize * 100));

        // Write sampled values (scaled by 10000 to match TypeScript implementation)
        writer.WriteUInt16BigEndian((ushort)Math.Round(SampledY * 10000));
        writer.WriteInt16BigEndian((short)Math.Round(SampledCb * 10000));
        writer.WriteInt16BigEndian((short)Math.Round(SampledCr * 10000));

        // Pad to 20 bytes total (we've written 18 bytes, need 2 more)
        writer.Pad(2);
        
        return memoryStream.ToArray();
    }
}