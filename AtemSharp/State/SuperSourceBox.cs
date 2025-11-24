using System.Drawing;

namespace AtemSharp.State;

public class SuperSourceBox
{
    public byte Id { get; set; }
    public bool Enabled { get; set; }
    public ushort Source { get; set; }
    public PointF Location { get; set; }
    public double Size { get; set; }
    public bool Cropped { get; set; }
    public double CropTop { get; set; }
    public double CropBottom { get; set; }
    public double CropLeft { get; set; }
    public double CropRight { get; set; }
}