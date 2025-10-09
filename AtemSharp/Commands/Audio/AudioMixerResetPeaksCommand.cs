using AtemSharp.Enums;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to reset audio mixer peak indicators
/// </summary>
[Command("RAMP")]
public class AudioMixerResetPeaksCommand : SerializedCommand
{
    private bool _all;
    private ushort _input;
    private bool _master;
    private bool _monitor;

    /// <summary>
    /// Create command with all reset options disabled
    /// </summary>
    public AudioMixerResetPeaksCommand()
    {
        // Initialize with all options false (no flags set)
        _all = false;
        _input = 0;
        _master = false;
        _monitor = false;
    }

    /// <summary>
    /// Reset all audio input peaks
    /// </summary>
    public bool All
    {
        get => _all;
        set
        {
            _all = value;
            Flag |= 1 << 0;
        }
    }

    /// <summary>
    /// Audio input index to reset peaks for (0-based)
    /// </summary>
    public ushort Input
    {
        get => _input;
        set
        {
            _input = value;
            Flag |= 1 << 1;
        }
    }

    /// <summary>
    /// Reset master audio peaks
    /// </summary>
    public bool Master
    {
        get => _master;
        set
        {
            _master = value;
            Flag |= 1 << 2;
        }
    }

    /// <summary>
    /// Reset monitor audio peaks
    /// </summary>
    public bool Monitor
    {
        get => _monitor;
        set
        {
            _monitor = value;
            Flag |= 1 << 3;
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data as byte array</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(8);
        using var writer = new BinaryWriter(memoryStream);
        
        writer.Write((byte)Flag);
        writer.WriteBoolean(All);
        writer.WriteUInt16(Input);
        writer.WriteBoolean(Master);
        writer.WriteBoolean(Monitor);
        writer.Pad(2); // Pad to match 8-byte buffer from TypeScript
        
        return memoryStream.ToArray();
    }
}