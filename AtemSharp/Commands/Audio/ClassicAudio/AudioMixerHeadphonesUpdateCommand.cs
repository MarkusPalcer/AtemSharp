using AtemSharp.State;

namespace AtemSharp.Commands.Audio.ClassicAudio;

[Command("AMHP")]
public partial class AudioMixerHeadphonesUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Gain in decibel
    /// </summary>
    [DeserializedField(0)] [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.UInt16ToDecibel)}")]
    private double _gain;

    /// <summary>
    /// Program out gain in decibel, -Infinity to +6dB
    /// </summary>
    [DeserializedField(2)] [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.UInt16ToDecibel)}")]
    private double _programOutGain;

    /// <summary>
    /// Sidetone gain in decibel, -Infinity to +6dB
    /// </summary>
    [DeserializedField(6)] [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.UInt16ToDecibel)}")]
    private double _sidetoneGain;

    /// <summary>
    /// Talkback gain in decibel, -Infinity to +6dB
    /// </summary>
    [DeserializedField(4)] [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.UInt16ToDecibel)}")]
    private double _talkbackGain;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var audio = state.GetClassicAudio();

        audio.Headphones.Gain = Gain;
        audio.Headphones.ProgramOutGain = ProgramOutGain;
        audio.Headphones.TalkbackGain = TalkbackGain;
        audio.Headphones.SidetoneGain = SidetoneGain;
    }
}
