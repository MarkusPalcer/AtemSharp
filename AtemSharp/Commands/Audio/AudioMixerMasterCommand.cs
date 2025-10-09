using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer master properties
/// </summary>
[Command("CAMM")]
public class AudioMixerMasterCommand : SerializedCommand
{
    private double _gain;
    private double _balance;
    private bool _followFadeToBlack;

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if classic audio not available</exception>
    public AudioMixerMasterCommand(AtemState currentState)
    {
        // If old state does not exist, set Properties (instead of backing fields) to default values,
        // so all flags are set (i.e. all values are to be applied by the ATEM)
        if (currentState.Audio?.Master == null)
        {
            throw new InvalidIdError("Classic Audio", "master");
        }

        var audioMaster = currentState.Audio.Master;
        
        // Initialize from current state (direct field access = no flags set)
        _gain = audioMaster.Gain;
        _balance = audioMaster.Balance;
        _followFadeToBlack = audioMaster.FollowFadeToBlack;
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
            Flag |= 1 << 0;
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
            Flag |= 1 << 1;
        }
    }

    /// <summary>
    /// Whether audio follows fade to black
    /// </summary>
    public bool FollowFadeToBlack
    {
        get => _followFadeToBlack;
        set
        {
            _followFadeToBlack = value;
            Flag |= 1 << 2;
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
        writer.Pad(1);
        writer.WriteUInt16BigEndian(Gain.DecibelToUInt16());
        writer.WriteInt16BigEndian(Balance.BalanceToInt16());
        writer.WriteBoolean(FollowFadeToBlack);
        writer.Pad(1);
        
        return memoryStream.ToArray();
    }
}