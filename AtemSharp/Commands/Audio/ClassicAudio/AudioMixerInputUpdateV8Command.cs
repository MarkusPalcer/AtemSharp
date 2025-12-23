using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Info;
using AtemSharp.State.Ports;

namespace AtemSharp.Commands.Audio.ClassicAudio;

[Command("AMIP", ProtocolVersion.V8_0)]
internal partial class AudioMixerInputUpdateV8Command : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _index;
    [DeserializedField(2)] private AudioSourceType _sourceType;
    [DeserializedField(6)] private ExternalPortType _portType;
    [DeserializedField(8)] private AudioMixOption _mixOption;

    [DeserializedField(10)] [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.UInt16ToDecibel)}")]
    private double _gain;

    [DeserializedField(12)]
    [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.Int16ToBalance)}")]
    [SerializedType(typeof(short))]
    private double _balance;

    [DeserializedField(14)] private bool _supportsRcaToXlrEnabled;
    [DeserializedField(15)] private bool _rcaToXlrEnabled;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var channel = state.GetClassicAudio().Channels.GetOrCreate(Index);
        channel.SourceType = SourceType;
        channel.PortType = PortType;
        channel.MixOption = MixOption;
        channel.Gain = Gain;
        channel.Balance = Balance;
        channel.SupportsRcaToXlrEnabled = SupportsRcaToXlrEnabled;
        channel.RcaToXlrEnabled = RcaToXlrEnabled;
    }
}
