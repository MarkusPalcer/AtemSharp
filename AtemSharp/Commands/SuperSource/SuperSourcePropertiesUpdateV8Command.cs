using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.State.Video.SuperSource;

namespace AtemSharp.Commands.SuperSource;

[Command("SSrc", ProtocolVersion.V8_0)]
public partial class SuperSourcePropertiesUpdateV8Command : IDeserializedCommand
{
    [DeserializedField(0)] private byte _superSourceId;
    [DeserializedField(2)] private ushort _artFillSource;
    [DeserializedField(4)] private ushort _artCutSource;
    [DeserializedField(6)] private ArtOption _artOption;
    [DeserializedField(7)] private bool _artPremultiplied;

    [DeserializedField(8)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _artClip;

    [DeserializedField(10)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _artGain;

    [DeserializedField(12)] private bool _artInvertKey;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var superSource = state.Video.SuperSources[SuperSourceId];
        superSource.FillSource = _artFillSource;
        superSource.CutSource = _artCutSource;
        superSource.Option = _artOption;
        superSource.PreMultiplied = _artPremultiplied;
        superSource.Clip = _artClip;
        superSource.Gain = _artGain;
        superSource.InvertKey = _artInvertKey;
    }
}
