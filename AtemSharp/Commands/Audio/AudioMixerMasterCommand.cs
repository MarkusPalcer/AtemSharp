using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer master properties
/// </summary>
public class AudioMixerMasterCommand : WritableCommand<object>
{
    public new static readonly Dictionary<string, int> MaskFlags = new()
    {
        { "gain", 1 << 0 },
        { "balance", 1 << 1 },
        { "followFadeToBlack", 1 << 2 },
    };

    public new static readonly string RawName = "CAMM";

    public AudioMixerMasterCommand() : base()
    {
    }

    /// <summary>
    /// Update audio mixer master properties
    /// </summary>
    /// <param name="gain">Gain in decibel</param>
    /// <param name="balance">Balance (-50 to +50)</param>
    /// <param name="followFadeToBlack">Follow fade to black</param>
    /// <returns>True if any properties were updated</returns>
    public bool UpdateProps(double? gain = null, double? balance = null, bool? followFadeToBlack = null)
    {
        var props = new Dictionary<string, object?>();
        
        if (gain.HasValue) props["gain"] = gain.Value;
        if (balance.HasValue) props["balance"] = balance.Value;
        if (followFadeToBlack.HasValue) props["followFadeToBlack"] = followFadeToBlack.Value;
        
        return UpdateProps(props);
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[8];
        
        // Flag
        buffer[0] = (byte)Flag;
        
        // Gain
        if (_properties.TryGetValue("gain", out var gain))
        {
            var gainValue = AtemUtil.DecibelToUInt16BE((double)(gain ?? 0.0));
            buffer[2] = (byte)(gainValue >> 8);
            buffer[3] = (byte)(gainValue & 0xFF);
        }
        
        // Balance
        if (_properties.TryGetValue("balance", out var balance))
        {
            var balanceValue = AtemUtil.BalanceToInt((double)(balance ?? 0.0));
            buffer[4] = (byte)(balanceValue >> 8);
            buffer[5] = (byte)(balanceValue & 0xFF);
        }
        
        // Follow fade to black
        if (_properties.TryGetValue("followFadeToBlack", out var followFadeToBlack))
        {
            buffer[6] = (bool)(followFadeToBlack ?? false) ? (byte)1 : (byte)0;
        }
        
        return buffer;
    }
}

/// <summary>
/// Command received when audio mixer master is updated
/// </summary>
public class AudioMixerMasterUpdateCommand : DeserializedCommand<ClassicAudioMasterChannel>
{
    public new static readonly string RawName = "AMMO";

    public AudioMixerMasterUpdateCommand(ClassicAudioMasterChannel properties) : base(properties)
    {
    }

    public static AudioMixerMasterUpdateCommand Deserialize(byte[] rawCommand)
    {
        var properties = new ClassicAudioMasterChannel
        {
            Gain = AtemUtil.UInt16BEToDecibel((ushort)((rawCommand[0] << 8) | rawCommand[1])),
            Balance = AtemUtil.IntToBalance((short)((rawCommand[2] << 8) | rawCommand[3])),
            FollowFadeToBlack = rawCommand[4] == 1
        };

        return new AudioMixerMasterUpdateCommand(properties);
    }

    public override string[] ApplyToState(AtemState state)
    {
        if (state.Audio == null)
        {
            throw new InvalidIdError("Classic Audio", "master");
        }

        if (state.Audio.Master == null)
        {
            state.Audio.Master = new ClassicAudioMasterChannel();
        }

        // Update properties
        state.Audio.Master.Gain = Properties.Gain;
        state.Audio.Master.Balance = Properties.Balance;
        state.Audio.Master.FollowFadeToBlack = Properties.FollowFadeToBlack;

        return new[] { "audio.master" };
    }
}