using AtemSharp.State;

namespace AtemSharp.Commands.Audio.ClassicAudio;

[Command("AMHP")]
internal partial class AudioMixerHeadphonesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.UInt16ToDecibel)}")]
    private double _gain;

    [DeserializedField(2)] [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.UInt16ToDecibel)}")]
    private double _programOutGain;

    [DeserializedField(6)] [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.UInt16ToDecibel)}")]
    private double _sidetoneGain;

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
