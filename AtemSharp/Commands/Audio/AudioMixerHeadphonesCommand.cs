using AtemSharp.Lib;
using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer headphones properties
/// </summary>
[Command("CAMH")]
[BufferSize(12)]
public partial class AudioMixerHeadphonesCommand(ClassicAudioState audio) : SerializedCommand
{
    /// <summary>
    /// Gain in decibel
    /// </summary>
    [SerializedField(2, 0)] [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _gain = audio.Headphones.Gain;

    /// <summary>
    /// Program out gain in decibel
    /// </summary>
    [SerializedField(4, 1)] [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _programOutGain = audio.Headphones.ProgramOutGain;

    /// <summary>
    /// Talkback gain in decibel
    /// </summary>
    [SerializedField(6, 2)] [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _talkbackGain = audio.Headphones.TalkbackGain;

    /// <summary>
    /// Sidetone gain in decibel
    /// </summary>
    [SerializedField(8, 3)] [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _sidetoneGain = audio.Headphones.SidetoneGain;
}
