using System.Drawing;
using AtemSharp.Enums;

namespace AtemSharp.State;

/// <summary>
/// Upstream keyer state containing key properties and settings
/// </summary>
public class UpstreamKeyer
{
    public byte MixEffectId { get; internal set; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public byte Id { get; internal set; }

    /// <summary>
    /// Whether the keyer is currently on air
    /// </summary>
    public bool OnAir { get; internal set; }

    /// <summary>
    /// Fill source input number
    /// </summary>
    public ushort FillSource { get; internal set; }

    /// <summary>
    /// Cut source input number
    /// </summary>
    public ushort CutSource { get; internal set; }

    /// <summary>
    /// Type of keying effect (Luma, Chroma, Pattern, DVE)
    /// </summary>
    public MixEffectKeyType KeyType { get; internal set; }

    /// <summary>
    /// Whether this keyer supports fly key functionality
    /// </summary>
    public bool CanFlyKey { get; internal set; }

    /// <summary>
    /// Whether fly key is currently enabled
    /// </summary>
    public bool FlyEnabled { get; internal set; }

    /// <summary>
    /// Mask settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerMaskSettings MaskSettings { get; } = new();

    /// <summary>
    /// Luma key settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerLumaSettings LumaSettings { get; } = new();

    /// <summary>
    /// Advanced chroma key settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerAdvancedChromaSettings AdvancedChromaSettings { get; } = new();

    /// <summary>
    /// DVE (Digital Video Effects) settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerDVESettings DigitalVideoEffectsSettings { get; } = new();

    /// <summary>
    /// Fly properties for the upstream keyer
    /// </summary>
    public UpstreamKeyerFlyProperties FlyProperties { get; } = new();

    public UpstreamKeyerFlyKeyframe[] Keyframes { get; }

    public UpstreamKeyer()
    {
        Keyframes =
        [
            new UpstreamKeyerFlyKeyframe
            {
                Id = 1,
                MixEffectId = MixEffectId,
                UpstreamKeyerId = Id
            },
            new UpstreamKeyerFlyKeyframe
            {
                Id = 2,
                MixEffectId = MixEffectId,
                UpstreamKeyerId = Id
            }

        ];
    }
}

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

public class MaskProperties
{
    public bool Enabled { get; internal set; }
    public double Top { get; internal set; }
    public double Bottom { get; internal set; }
    public double Left { get; internal set; }
    public double Right { get; internal set; }
}

public class BorderProperties
{
    public double OuterWidth { get; internal set; }
    public double InnerWidth { get; internal set; }
    public byte OuterSoftness { get; internal set; }
    public byte InnerSoftness { get; internal set; }
    public byte BevelSoftness { get; internal set; }
    public byte BevelPosition { get; internal set; }
    public byte Opacity { get; internal set; }
    public double Hue { get; internal set; }
    public double Saturation { get; internal set; }
    public double Luma { get; internal set; }
}
