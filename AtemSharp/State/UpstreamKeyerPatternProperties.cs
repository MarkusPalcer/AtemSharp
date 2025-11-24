using System.Drawing;

namespace AtemSharp.State;

public class UpstreamKeyerPatternProperties {
    public UpstreamKeyerPatternStyle Style { get; internal set; }
    public double Size { get; set; }
    public double Symmetry { get; set; }
    public double Softness { get; set; }
    public PointF Location { get; set; }
    public bool Invert { get; set; }
}
