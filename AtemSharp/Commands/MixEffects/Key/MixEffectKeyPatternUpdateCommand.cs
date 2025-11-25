using System.Drawing;
using AtemSharp.State;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("KePt")]
public partial class MixEffectKeyPatternUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _mixEffectId;

    [DeserializedField(1)]
    private byte _keyerId;

    [DeserializedField(2)] private UpstreamKeyerPatternStyle _style;
    [DeserializedField(4)] [SerializedType(typeof(ushort))] [ScalingFactor(100)] private double _size;
    [DeserializedField(6)] [SerializedType(typeof(ushort))] [ScalingFactor(100)] private double _symmetry;

    [DeserializedField(8)] [SerializedType(typeof(ushort))] [ScalingFactor(100)]
    private double _softness;

    [DeserializedField(10)] [SerializedType(typeof(ushort))] [ScalingFactor(10000)]
    private double _positionX;
    [DeserializedField(12)] [SerializedType(typeof(ushort))] [ScalingFactor(10000)]
    private double _positionY;

    [DeserializedField(14)] private bool _invert;

    public void ApplyToState(AtemState state)
    {
        var properties = state.Video.MixEffects[_mixEffectId].UpstreamKeyers[_keyerId].Pattern;

        properties.Style = _style;
        properties.Size = _size;
        properties.Symmetry = _symmetry;
        properties.Softness = _softness;
        properties.Location = new PointF((float)_positionX, (float)_positionY);
        properties.Invert = _invert;
    }
}
