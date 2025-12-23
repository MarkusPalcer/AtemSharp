using System.Drawing;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Used to set the pattern settings of an UpstreamKeyer
/// </summary>
[Command("CKPt")]
[BufferSize(16)]
public partial class MixEffectKeyPatternCommand(UpstreamKeyer keyer) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _mixEffectId = keyer.MixEffectId;

    [SerializedField(2)] [NoProperty] private readonly byte _keyerId = keyer.Id;

    [SerializedField(3, 0)] private UpstreamKeyerPatternStyle _style = keyer.Pattern.Style;

    [SerializedField(4, 1)] [SerializedType(typeof(ushort))] [ScalingFactor(100)]
    private double _size = keyer.Pattern.Size;

    [SerializedField(6, 2)] [SerializedType(typeof(ushort))] [ScalingFactor(100)]
    private double _symmetry = keyer.Pattern.Symmetry;

    [SerializedField(8, 3)] [SerializedType(typeof(ushort))] [ScalingFactor(100)]
    private double _softness = keyer.Pattern.Softness;

    [SerializedField(10, 4)] [SerializedType(typeof(ushort))] [ScalingFactor(10000)]
    private double _positionX = keyer.Pattern.Location.X;

    [SerializedField(12, 5)] [SerializedType(typeof(ushort))] [ScalingFactor(10000)]
    private double _positionY = keyer.Pattern.Location.Y;

    [SerializedField(14, 6)] private bool _invert = keyer.Pattern.Invert;

    public PointF Location
    {
        get => new((float)_positionX, (float)_positionY);
        set
        {
            PositionX = value.X;
            PositionY = value.Y;
        }
    }
}
