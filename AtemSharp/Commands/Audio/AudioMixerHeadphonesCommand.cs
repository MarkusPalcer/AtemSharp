using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer headphones properties
/// </summary>
[Command("CAMH")]
[BufferSize(12)]
public partial class AudioMixerHeadphonesCommand : SerializedCommand
{
    /// <summary>
    /// Gain in decibel
    /// </summary>
    [SerializedField(2, 0)] [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.DecibelToUInt16)}")]
    private double _gain;

    /// <summary>
    /// Program out gain in decibel
    /// </summary>
    [SerializedField(4, 1)] [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.DecibelToUInt16)}")]
    private double _programOutGain;

    /// <summary>
    /// Talkback gain in decibel
    /// </summary>
    [SerializedField(6, 2)] [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.DecibelToUInt16)}")]
    private double _talkbackGain;

    /// <summary>
    /// Sidetone gain in decibel
    /// </summary>
    [SerializedField(8, 3)] [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.DecibelToUInt16)}")]
    private double _sidetoneGain;

    public AudioMixerHeadphonesCommand(AtemState currentState)
    {
        var audio = currentState.GetClassicAudio();
        // If the audio state or headphones do not exist, initialize to default values
        // by setting the properties, thus setting the changed-flag for each property
        if (audio.Headphones is null)
        {
            Gain = 0.0;
            ProgramOutGain = 0.0;
            TalkbackGain = 0.0;
            SidetoneGain = 0.0;
            return;
        }

        _gain = audio.Headphones.Gain;
        _programOutGain = audio.Headphones.ProgramOutGain;
        _talkbackGain = audio.Headphones.TalkbackGain;
        _sidetoneGain = audio.Headphones.SidetoneGain;
    }
}
