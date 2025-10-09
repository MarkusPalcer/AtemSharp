using AtemSharp.Enums;
using AtemSharp.Enums.Audio;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer input properties
/// </summary>
[Command("CAMI")]
public class AudioMixerInputCommand : SerializedCommand
{
    private AudioMixOption _mixOption;
    private double _gain;
    private double _balance;
    private bool _rcaToXlrEnabled;

    /// <summary>
    /// Audio input index
    /// </summary>
    public ushort Index { get; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="index">Audio input index (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if audio input not available</exception>
    public AudioMixerInputCommand(ushort index, AtemState currentState)
    {
        Index = index;

        // Validate audio input exists (like TypeScript update command)
        if (currentState.Audio?.Channels == null || !currentState.Audio.Channels.ContainsKey(index))
        {
            throw new InvalidIdError("Classic Audio Input", index);
        }

        var audioChannel = currentState.Audio.Channels[index];
        if (audioChannel == null)
        {
            throw new InvalidIdError("Classic Audio Input", index);
        }

        // Initialize from current state (direct field access = no flags)
        _mixOption = audioChannel.MixOption;
        _gain = audioChannel.Gain;
        _balance = audioChannel.Balance;
        _rcaToXlrEnabled = audioChannel.RcaToXlrEnabled;
    }

    /// <summary>
    /// Audio mix option (Off, On, AfterFader)
    /// </summary>
    public AudioMixOption MixOption
    {
        get => _mixOption;
        set
        {
            _mixOption = value;
            Flag |= 1 << 0;
        }
    }

    /// <summary>
    /// Audio gain in decibels (-60.0 to +6.0)
    /// </summary>
    public double Gain
    {
        get => _gain;
        set
        {
            if (value < -60.0 || value > 6.0)
                throw new ArgumentOutOfRangeException(nameof(value), "Gain must be between -60.0 and +6.0 decibels");
            
            _gain = value;
            Flag |= 1 << 1;
        }
    }

    /// <summary>
    /// Audio balance (-50.0 to +50.0, where 0 is center)
    /// </summary>
    public double Balance
    {
        get => _balance;
        set
        {
            if (value < -50.0 || value > 50.0)
                throw new ArgumentOutOfRangeException(nameof(value), "Balance must be between -50.0 and +50.0");
            
            _balance = value;
            Flag |= 1 << 2;
        }
    }

    /// <summary>
    /// Whether RCA to XLR conversion is enabled
    /// </summary>
    public bool RcaToXlrEnabled
    {
        get => _rcaToXlrEnabled;
        set
        {
            _rcaToXlrEnabled = value;
            Flag |= 1 << 3;
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data as stream</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
	    using var memoryStream = new MemoryStream(12);
	    using var writer = new BinaryWriter(memoryStream);
        
	    writer.Write((byte)Flag);
	    writer.Pad(1);
		writer.WriteUInt16BigEndian(Index);
		writer.Write((byte)MixOption);
		writer.Pad(1);
		writer.WriteUInt16BigEndian(Gain.DecibelToUInt16());
		writer.WriteInt16BigEndian(Balance.BalanceToInt16());
		writer.WriteBoolean(RcaToXlrEnabled);
		writer.Pad(1);
        
        return memoryStream.ToArray();
    }
}