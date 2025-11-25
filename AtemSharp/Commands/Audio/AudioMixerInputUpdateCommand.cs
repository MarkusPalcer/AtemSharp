using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Ports;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Update command for audio mixer input properties
/// </summary>
[Command("AMIP")]
public partial class AudioMixerInputUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Audio input index
    /// </summary>
    [DeserializedField(0)] private ushort _index;

    /// <summary>
    /// Audio source type (readonly)
    /// </summary>
    [DeserializedField(2)] private AudioSourceType _sourceType;


    /// <summary>
    /// External port type
    /// </summary>
    [DeserializedField(6)] private ExternalPortType _portType;

    /// <summary>
    /// Audio mix option
    /// </summary>
    [DeserializedField(8)] private AudioMixOption _mixOption;

    /// <summary>
    /// Gain in decibel
    /// </summary>
    [DeserializedField(10)] [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.UInt16ToDecibel)}")]
    private double _gain;

    /// <summary>
    /// Balance, -50 to +50
    /// </summary>
    [DeserializedField(12)] [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.Int16ToBalance)}")] [SerializedType(typeof(short))]
    private double _balance;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var audio = state.GetClassicAudio();

        audio.Channels[Index] = new ClassicAudioChannel
        {
            Id = Index,
            SourceType = SourceType,
            PortType = _portType,
            MixOption = _mixOption,
            Gain = Gain,
            Balance = Balance,
            RcaToXlrEnabled = false,
            SupportsRcaToXlrEnabled = false
        };
    }
}
