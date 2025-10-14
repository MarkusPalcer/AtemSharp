namespace AtemSharp.State;

/// <summary>
/// Advanced chroma key properties for upstream keyer
/// </summary>
public class UpstreamKeyerAdvancedChromaProperties
{
    /// <summary>
    /// Foreground level value
    /// </summary>
    public double ForegroundLevel { get; set; }

    /// <summary>
    /// Background level value
    /// </summary>
    public double BackgroundLevel { get; set; }

    /// <summary>
    /// Key edge value
    /// </summary>
    public double KeyEdge { get; set; }

    /// <summary>
    /// Spill suppression value
    /// </summary>
    public double SpillSuppression { get; set; }

    /// <summary>
    /// Flare suppression value
    /// </summary>
    public double FlareSuppression { get; set; }

    /// <summary>
    /// Brightness adjustment value
    /// </summary>
    public double Brightness { get; set; }

    /// <summary>
    /// Contrast adjustment value
    /// </summary>
    public double Contrast { get; set; }

    /// <summary>
    /// Saturation adjustment value
    /// </summary>
    public double Saturation { get; set; }

    /// <summary>
    /// Red color adjustment value
    /// </summary>
    public double Red { get; set; }

    /// <summary>
    /// Green color adjustment value
    /// </summary>
    public double Green { get; set; }

    /// <summary>
    /// Blue color adjustment value
    /// </summary>
    public double Blue { get; set; }
}