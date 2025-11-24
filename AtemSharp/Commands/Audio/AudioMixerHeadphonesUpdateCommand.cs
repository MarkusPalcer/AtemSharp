using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio;

[Command("AMHP")]
public partial class AudioMixerHeadphonesUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Gain in decibel
    /// </summary>
    [DeserializedField(0)] [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.UInt16ToDecibel)}")]
    private double _gain;

    /// <summary>
    /// Program out gain in decibel, -Infinity to +6dB
    /// </summary>
    [DeserializedField(2)] [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.UInt16ToDecibel)}")]
    private double _programOutGain;

    /// <summary>
    /// Sidetone gain in decibel, -Infinity to +6dB
    /// </summary>
    [DeserializedField(6)] [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.UInt16ToDecibel)}")]
    private double _sidetoneGain;

    /// <summary>
    /// Talkback gain in decibel, -Infinity to +6dB
    /// </summary>
    [DeserializedField(4)] [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.UInt16ToDecibel)}")]
    private double _talkbackGain;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var audio = state.GetClassicAudio();

        audio.Headphones ??= new ClassicAudioHeadphoneOutputChannel();
        audio.Headphones.Gain = Gain;
        audio.Headphones.ProgramOutGain = ProgramOutGain;
        audio.Headphones.TalkbackGain = TalkbackGain;
        audio.Headphones.SidetoneGain = SidetoneGain;
    }
}
