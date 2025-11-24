using System.Drawing;

namespace AtemSharp.State;

public class SuperSourceBox
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
}
