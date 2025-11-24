using System.Drawing;

namespace AtemSharp.State;

public class UpstreamKeyerPatternProperties {
    public UpstreamKeyerPatternStyle Style { get; internal set; }
    public double Size { get; internal set; }
    public double Symmetry { get; internal set; }
    public double Softness { get; internal set; }
    public PointF Location { get; internal set; }
    public bool Invert { get; internal set; }
}
