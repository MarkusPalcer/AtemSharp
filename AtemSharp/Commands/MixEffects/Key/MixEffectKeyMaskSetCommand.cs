using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("CKMs")]
[BufferSize(12)]
public partial class MixEffectKeyMaskSetCommand(UpstreamKeyer keyer) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _mixEffectId = keyer.MixEffectId;

    [SerializedField(2)] [NoProperty] private readonly byte _keyerId = keyer.Id;

    [SerializedField(3, 0)] private bool _enabled = keyer.Mask.Enabled;

    [SerializedField(4, 1)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _top = keyer.Mask.Top;

    [SerializedField(6, 2)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _bottom = keyer.Mask.Bottom;

    [SerializedField(8, 3)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _left = keyer.Mask.Left;

    [SerializedField(10, 4)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _right = keyer.Mask.Right;
}
