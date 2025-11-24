using AtemSharp.Enums;
using AtemSharp.Enums.Audio;
using AtemSharp.Enums.Ports;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Update command for audio mixer input properties (V8+ protocol version)
/// </summary>
[Command("AMIP", ProtocolVersion.V8_0)]
public partial class AudioMixerInputUpdateV8Command : IDeserializedCommand
{
    /// <summary>
    /// Audio input index
    /// </summary>
    [DeserializedField(0)]
    private ushort _index;

    /// <summary>
    /// Audio source type (readonly)
    /// </summary>
    [DeserializedField(2)]
    private AudioSourceType _sourceType;

    /// <summary>
    /// External port type
    /// </summary>
    [DeserializedField(6)]
    private ExternalPortType _portType;

    /// <summary>
    /// Audio mix option
    /// </summary>
    [DeserializedField(8)]
    private AudioMixOption _mixOption;

    /// <summary>
    /// Gain in decibel, -Infinity to +6dB
    /// </summary>
    [DeserializedField(10)]
    [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.UInt16ToDecibel)}")]
    private double _gain;

    /// <summary>
    /// Balance, -50 to +50
    /// </summary>
    [DeserializedField(12)]
    [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.Int16ToBalance)}")]
    [SerializedType(typeof(short))]
    private double _balance;

    /// <summary>
    /// Whether this channel supports RCA to XLR enabled setting (readonly)
    /// </summary>
    [DeserializedField(14)]
    private bool _supportsRcaToXlrEnabled;

    /// <summary>
    /// RCA to XLR enabled
    /// </summary>
    [DeserializedField(15)]
    private bool _rcaToXlrEnabled;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var audio = state.GetClassicAudio();

        audio.Channels[Index] = new ClassicAudioChannel
        {
            SourceType = SourceType,
            PortType = PortType,
            MixOption = MixOption,
            Gain = Gain,
            Balance = Balance,
            SupportsRcaToXlrEnabled = SupportsRcaToXlrEnabled,
            RcaToXlrEnabled = RcaToXlrEnabled
        };
    }
}
