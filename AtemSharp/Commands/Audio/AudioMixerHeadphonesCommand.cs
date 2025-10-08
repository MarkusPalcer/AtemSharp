using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer headphones properties
/// </summary>
public class AudioMixerHeadphonesCommand : WritableCommand<object>
{
    public new static readonly Dictionary<string, int> MaskFlags = new()
    {
        { "gain", 1 << 0 },
        { "programOutGain", 1 << 1 },
        { "talkbackGain", 1 << 2 },
        { "sidetoneGain", 1 << 3 },
    };

    public new static readonly string RawName = "CAMH";

    public AudioMixerHeadphonesCommand() : base()
    {
    }

    /// <summary>
    /// Update audio mixer headphones properties
    /// </summary>
    /// <param name="gain">Gain in decibel</param>
    /// <param name="programOutGain">Program out gain in decibel</param>
    /// <param name="talkbackGain">Talkback gain in decibel</param>
    /// <param name="sidetoneGain">Sidetone gain in decibel</param>
    /// <returns>True if any properties were updated</returns>
    public bool UpdateProps(double? gain = null, double? programOutGain = null, double? talkbackGain = null, double? sidetoneGain = null)
    {
        var props = new Dictionary<string, object?>();
        
        if (gain.HasValue) props["gain"] = gain.Value;
        if (programOutGain.HasValue) props["programOutGain"] = programOutGain.Value;
        if (talkbackGain.HasValue) props["talkbackGain"] = talkbackGain.Value;
        if (sidetoneGain.HasValue) props["sidetoneGain"] = sidetoneGain.Value;
        
        return UpdateProps(props);
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[12];
        
        // Flag
        buffer[0] = (byte)Flag;
        
        // Gain
        if (_properties.TryGetValue("gain", out var gain))
        {
            var gainValue = AtemUtil.DecibelToUInt16BE((double)(gain ?? 0.0));
            buffer[2] = (byte)(gainValue >> 8);
            buffer[3] = (byte)(gainValue & 0xFF);
        }
        
        // Program out gain
        if (_properties.TryGetValue("programOutGain", out var programOutGain))
        {
            var gainValue = AtemUtil.DecibelToUInt16BE((double)(programOutGain ?? 0.0));
            buffer[4] = (byte)(gainValue >> 8);
            buffer[5] = (byte)(gainValue & 0xFF);
        }
        
        // Talkback gain
        if (_properties.TryGetValue("talkbackGain", out var talkbackGain))
        {
            var gainValue = AtemUtil.DecibelToUInt16BE((double)(talkbackGain ?? 0.0));
            buffer[6] = (byte)(gainValue >> 8);
            buffer[7] = (byte)(gainValue & 0xFF);
        }
        
        // Sidetone gain
        if (_properties.TryGetValue("sidetoneGain", out var sidetoneGain))
        {
            var gainValue = AtemUtil.DecibelToUInt16BE((double)(sidetoneGain ?? 0.0));
            buffer[8] = (byte)(gainValue >> 8);
            buffer[9] = (byte)(gainValue & 0xFF);
        }
        
        return buffer;
    }
}

/// <summary>
/// Command received when audio mixer headphones is updated
/// </summary>
public class AudioMixerHeadphonesUpdateCommand : DeserializedCommand<ClassicAudioHeadphoneOutputChannel>
{
    public new static readonly string RawName = "AMHP";

    public AudioMixerHeadphonesUpdateCommand(ClassicAudioHeadphoneOutputChannel properties) : base(properties)
    {
    }

    public static AudioMixerHeadphonesUpdateCommand Deserialize(byte[] rawCommand)
    {
        var properties = new ClassicAudioHeadphoneOutputChannel
        {
            Gain = AtemUtil.UInt16BEToDecibel((ushort)((rawCommand[0] << 8) | rawCommand[1])),
            ProgramOutGain = AtemUtil.UInt16BEToDecibel((ushort)((rawCommand[2] << 8) | rawCommand[3])),
            TalkbackGain = AtemUtil.UInt16BEToDecibel((ushort)((rawCommand[4] << 8) | rawCommand[5])),
            SidetoneGain = AtemUtil.UInt16BEToDecibel((ushort)((rawCommand[6] << 8) | rawCommand[7]))
        };

        return new AudioMixerHeadphonesUpdateCommand(properties);
    }

    public override string[] ApplyToState(AtemState state)
    {
        if (state.Audio == null)
        {
            throw new InvalidIdError("Classic Audio", "headphones");
        }

        if (state.Audio.Headphones == null)
        {
            state.Audio.Headphones = new ClassicAudioHeadphoneOutputChannel();
        }

        // Update properties
        state.Audio.Headphones.Gain = Properties.Gain;
        state.Audio.Headphones.ProgramOutGain = Properties.ProgramOutGain;
        state.Audio.Headphones.TalkbackGain = Properties.TalkbackGain;
        state.Audio.Headphones.SidetoneGain = Properties.SidetoneGain;

        return new[] { "audio.headphones" };
    }
}