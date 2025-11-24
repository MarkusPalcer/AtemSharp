namespace AtemSharp.State;

/// <summary>
/// Advanced chroma key properties for upstream keyer
/// </summary>
public class UpstreamKeyerAdvancedChromaProperties
{
    /// <summary>
    /// Foreground level value
    /// </summary>
    public double ForegroundLevel { get; internal set; }

    /// <summary>
    /// Background level value
    /// </summary>
    public double BackgroundLevel { get; internal set; }

    /// <summary>
    /// Key edge value
    /// </summary>
    public double KeyEdge { get; internal set; }

    /// <summary>
    /// Spill suppression value
    /// </summary>
    public double SpillSuppression { get; internal set; }

    /// <summary>
    /// Flare suppression value
    /// </summary>
    public double FlareSuppression { get; internal set; }

    /// <summary>
    /// Brightness adjustment value
    /// </summary>
    public double Brightness { get; internal set; }

    /// <summary>
    /// Contrast adjustment value
    /// </summary>
    public double Contrast { get; internal set; }

    /// <summary>
    /// Saturation adjustment value
    /// </summary>
    public double Saturation { get; internal set; }

    /// <summary>
    /// Red color adjustment value
    /// </summary>
    public double Red { get; internal set; }

    /// <summary>
    /// Green color adjustment value
    /// </summary>
    public double Green { get; internal set; }

    /// <summary>
    /// Blue color adjustment value
    /// </summary>
    public double Blue { get; internal set; }
}
