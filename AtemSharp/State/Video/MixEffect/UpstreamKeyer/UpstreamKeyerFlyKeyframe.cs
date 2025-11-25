using System.Drawing;
using AtemSharp.State.Border;

namespace AtemSharp.State.Video.MixEffect.UpstreamKeyer;

public class UpstreamKeyerFlyKeyframe
{
    public byte UpstreamKeyerId { get; internal set; }

    public byte MixEffectId { get; internal set; }

    public byte Id { get; internal set; }
    public SizeF Size { get; internal set; }
    public PointF Location { get; internal set; }
    public double Rotation { get; internal set; }
    public BorderProperties Border { get; internal set; } = new();
    public double LightSourceDirection { get; internal set; }
    public byte LightSourceAltitude { get; internal set; }
    public MaskProperties Mask { get; internal set; } = new();
}