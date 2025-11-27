using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Video.MixEffect.UpstreamKeyer;

/// <summary>
/// Advanced chroma key sample settings for upstream keyer
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class UpstreamKeyerAdvancedChromaSample
{
    /// <summary>
    /// Whether the cursor is enabled
    /// </summary>
    public bool EnableCursor { get; internal set; }

    /// <summary>
    /// Whether to show preview
    /// </summary>
    public bool Preview { get; internal set; }

    /// <summary>
    /// Cursor X position
    /// </summary>
    public double CursorX { get; internal set; }

    /// <summary>
    /// Cursor Y position
    /// </summary>
    public double CursorY { get; internal set; }

    /// <summary>
    /// Cursor size
    /// </summary>
    public double CursorSize { get; internal set; }

    /// <summary>
    /// Sampled Y (luminance) value
    /// </summary>
    public double SampledY { get; internal set; }

    /// <summary>
    /// Sampled Cb (blue-difference chroma) value
    /// </summary>
    public double SampledCb { get; internal set; }

    /// <summary>
    /// Sampled Cr (red-difference chroma) value
    /// </summary>
    public double SampledCr { get; internal set; }
}
