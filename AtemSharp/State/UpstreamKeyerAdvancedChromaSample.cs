namespace AtemSharp.State;

/// <summary>
/// Advanced chroma key sample settings for upstream keyer
/// </summary>
public class UpstreamKeyerAdvancedChromaSample
{
    /// <summary>
    /// Whether the cursor is enabled
    /// </summary>
    public bool EnableCursor { get; set; }

    /// <summary>
    /// Whether to show preview
    /// </summary>
    public bool Preview { get; set; }

    /// <summary>
    /// Cursor X position
    /// </summary>
    public double CursorX { get; set; }

    /// <summary>
    /// Cursor Y position
    /// </summary>
    public double CursorY { get; set; }

    /// <summary>
    /// Cursor size
    /// </summary>
    public double CursorSize { get; set; }

    /// <summary>
    /// Sampled Y (luminance) value
    /// </summary>
    public double SampledY { get; set; }

    /// <summary>
    /// Sampled Cb (blue-difference chroma) value
    /// </summary>
    public double SampledCb { get; set; }

    /// <summary>
    /// Sampled Cr (red-difference chroma) value
    /// </summary>
    public double SampledCr { get; set; }
}