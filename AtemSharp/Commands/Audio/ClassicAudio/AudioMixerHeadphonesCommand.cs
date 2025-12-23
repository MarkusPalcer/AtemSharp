using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio.ClassicAudio;

/// <summary>
/// Command to update audio mixer headphone gains
/// </summary>
[Command("CAMH")]
[BufferSize(12)]
public partial class AudioMixerHeadphonesCommand(ClassicAudioState audio) : SerializedCommand
{
    [SerializedField(2, 0)] [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _gain = audio.Headphones.Gain;

    [SerializedField(4, 1)] [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _programOutGain = audio.Headphones.ProgramOutGain;

    [SerializedField(6, 2)] [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _talkbackGain = audio.Headphones.TalkbackGain;

    [SerializedField(8, 3)] [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _sidetoneGain = audio.Headphones.SidetoneGain;
}
