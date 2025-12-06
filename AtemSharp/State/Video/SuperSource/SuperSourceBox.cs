using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AtemSharp.State.Video.SuperSource;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class SuperSourceBox : ItemWithId<byte>
{
    public byte SuperSourceId { get; internal set; }
    public byte Id { get; internal set; }
    public bool Enabled { get; internal set; }
    public ushort Source { get; internal set; }
    public PointF Location { get; internal set; }
    public double Size { get; internal set; }
    public bool Cropped { get; internal set; }
    public double CropTop { get; internal set; }
    public double CropBottom { get; internal set; }
    public double CropLeft { get; internal set; }
    public double CropRight { get; internal set; }
    internal override void SetId(byte id) =>  Id = id;
}
