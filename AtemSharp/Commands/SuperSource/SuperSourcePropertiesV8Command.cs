using AtemSharp.State.Info;
using AtemSharp.State.Video.SuperSource;

namespace AtemSharp.Commands.SuperSource;

[Command("CSSc", ProtocolVersion.V8_0)]
[BufferSize(16)]
public partial class SuperSourcePropertiesV8Command(State.Video.SuperSource.SuperSource source) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _superSourceId = source.Id;
    [SerializedField(2,0)] private ushort _artFillSource = source.FillSource;
    [SerializedField(4,1)] private ushort _artCutSource = source.CutSource;
    [SerializedField(6,2)] private ArtOption _artOption = source.Option;
    [SerializedField(7,3)] private bool _artPremultiplied = source.PreMultiplied;

    [SerializedField(8,4)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _artClip = source.Clip;

    [SerializedField(10,5)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _artGain = source.Gain;

    [SerializedField(12,6)] private bool _artInvertKey = source.InvertKey;
}
