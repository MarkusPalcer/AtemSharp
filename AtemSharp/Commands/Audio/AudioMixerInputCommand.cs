using System;
using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer input properties
/// </summary>
public class AudioMixerInputCommand : WritableCommand<object>
{
    public new static readonly Dictionary<string, int> MaskFlags = new()
    {
        { "mixOption", 1 << 0 },
        { "gain", 1 << 1 },
        { "balance", 1 << 2 },
        { "rcaToXlrEnabled", 1 << 3 },
    };

    public new static readonly string RawName = "CAMI";

    public int Index { get; }

    public AudioMixerInputCommand(int index) : base()
    {
        Index = index;
    }

    /// <summary>
    /// Update audio mixer input properties
    /// </summary>
    /// <param name="mixOption">Audio mix option</param>
    /// <param name="gain">Gain in decibel</param>
    /// <param name="balance">Balance (-50 to +50)</param>
    /// <param name="rcaToXlrEnabled">RCA to XLR enabled</param>
    /// <returns>True if any properties were updated</returns>
    public bool UpdateProps(AudioMixOption? mixOption = null, double? gain = null, double? balance = null, bool? rcaToXlrEnabled = null)
    {
        var props = new Dictionary<string, object?>();
        
        if (mixOption.HasValue) props["mixOption"] = mixOption.Value;
        if (gain.HasValue) props["gain"] = gain.Value;
        if (balance.HasValue) props["balance"] = balance.Value;
        if (rcaToXlrEnabled.HasValue) props["rcaToXlrEnabled"] = rcaToXlrEnabled.Value;
        
        return UpdateProps(props);
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[12];
        
        // Flag
        buffer[0] = (byte)Flag;
        
        // Index
        buffer[2] = (byte)(Index >> 8);
        buffer[3] = (byte)(Index & 0xFF);
        
        // Mix option
        if (_properties.TryGetValue("mixOption", out var mixOption))
        {
            buffer[4] = (byte)((AudioMixOption)(mixOption ?? AudioMixOption.Off));
        }
        
        // Gain
        if (_properties.TryGetValue("gain", out var gain))
        {
            var gainValue = AtemUtil.DecibelToUInt16BE((double)(gain ?? 0.0));
            buffer[6] = (byte)(gainValue >> 8);
            buffer[7] = (byte)(gainValue & 0xFF);
        }
        
        // Balance
        if (_properties.TryGetValue("balance", out var balance))
        {
            var balanceValue = AtemUtil.BalanceToInt((double)(balance ?? 0.0));
            buffer[8] = (byte)(balanceValue >> 8);
            buffer[9] = (byte)(balanceValue & 0xFF);
        }
        
        // RCA to XLR enabled
        if (_properties.TryGetValue("rcaToXlrEnabled", out var rcaToXlr))
        {
            buffer[10] = (bool)(rcaToXlr ?? false) ? (byte)1 : (byte)0;
        }
        
        return buffer;
    }
}

/// <summary>
/// Command received when audio mixer input is updated
/// </summary>
public class AudioMixerInputUpdateCommand : DeserializedCommand<ClassicAudioChannel>
{
    public new static readonly string RawName = "AMIP";

    public int Index { get; }

    public AudioMixerInputUpdateCommand(int index, ClassicAudioChannel properties) : base(properties)
    {
        Index = index;
    }

    public static AudioMixerInputUpdateCommand Deserialize(byte[] rawCommand)
    {
        var index = (rawCommand[0] << 8) | rawCommand[1];
        
        var properties = new ClassicAudioChannel
        {
            SourceType = (AudioSourceType)rawCommand[2],
            PortType = (ExternalPortType)((rawCommand[6] << 8) | rawCommand[7]),
            MixOption = (AudioMixOption)rawCommand[8],
            Gain = AtemUtil.UInt16BEToDecibel((ushort)((rawCommand[10] << 8) | rawCommand[11])),
            Balance = AtemUtil.IntToBalance((short)((rawCommand[12] << 8) | rawCommand[13])),
            RcaToXlrEnabled = false,
            SupportsRcaToXlrEnabled = false
        };

        return new AudioMixerInputUpdateCommand(index, properties);
    }

    public override string[] ApplyToState(AtemState state)
    {
        if (state.Audio == null)
        {
            throw new InvalidIdError("Classic Audio", Index);
        }

        state.Audio.Channels[Index] = Properties;
        return new[] { $"audio.channels.{Index}" };
    }
}

/// <summary>
/// Audio mixer input update command for V8+ protocol
/// </summary>
public class AudioMixerInputUpdateV8Command : DeserializedCommand<ClassicAudioChannel>
{
    public new static readonly ProtocolVersion MinimumVersion = ProtocolVersion.V8_0;
    public new static readonly string RawName = "AMIP";

    public int Index { get; }

    public AudioMixerInputUpdateV8Command(int index, ClassicAudioChannel properties) : base(properties)
    {
        Index = index;
    }

    public static AudioMixerInputUpdateV8Command Deserialize(byte[] rawCommand)
    {
        var index = (rawCommand[0] << 8) | rawCommand[1];
        
        var properties = new ClassicAudioChannel
        {
            SourceType = (AudioSourceType)rawCommand[2],
            PortType = (ExternalPortType)((rawCommand[6] << 8) | rawCommand[7]),
            MixOption = (AudioMixOption)rawCommand[8],
            Gain = AtemUtil.UInt16BEToDecibel((ushort)((rawCommand[10] << 8) | rawCommand[11])),
            Balance = AtemUtil.IntToBalance((short)((rawCommand[12] << 8) | rawCommand[13])),
            RcaToXlrEnabled = rawCommand[14] != 0,
            SupportsRcaToXlrEnabled = rawCommand[15] != 0
        };

        return new AudioMixerInputUpdateV8Command(index, properties);
    }

    public override string[] ApplyToState(AtemState state)
    {
        if (state.Audio == null)
        {
            throw new InvalidIdError("Classic Audio", Index);
        }

        state.Audio.Channels[Index] = Properties;
        return new[] { $"audio.channels.{Index}" };
    }
}