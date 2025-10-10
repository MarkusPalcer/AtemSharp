using AtemSharp.Enums;

namespace AtemSharp.State;

/// <summary>
/// Information about a supported video mode on the ATEM device
/// </summary>
public class SupportedVideoMode
{
    /// <summary>
    /// The video mode
    /// </summary>
    public VideoMode Mode { get; set; }

    /// <summary>
    /// Whether this mode requires device reconfiguration to activate
    /// </summary>
    public bool RequiresReconfig { get; set; }

    /// <summary>
    /// Video modes supported for multiviewer output when in this mode
    /// </summary>
    public VideoMode[] MultiviewerModes { get; set; } = [];

    /// <summary>
    /// Video modes supported for down-converted outputs when in this mode
    /// </summary>
    public VideoMode[] DownConvertModes { get; set; } = [];
}