using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Ports;

namespace AtemSharp.Commands.Audio.ClassicAudio;

[Command("AMIP")]
internal partial class AudioMixerInputUpdateCommand : IDeserializedCommand
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

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var channel = state.GetClassicAudio().Channels.GetOrCreate(Index);
        channel.SourceType = SourceType;
        channel.PortType = _portType;
        channel.MixOption = _mixOption;
        channel.Gain = Gain;
        channel.Balance = Balance;
        channel.RcaToXlrEnabled = false;
        channel.SupportsRcaToXlrEnabled = false;
    }
}
