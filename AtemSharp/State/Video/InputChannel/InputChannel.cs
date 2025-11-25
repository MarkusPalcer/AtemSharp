using AtemSharp.State.Ports;

namespace AtemSharp.State.Video.InputChannel;

/// <summary>
/// Input channel configuration and properties
/// </summary>
public class InputChannel
{
    /// <summary>
    /// Input identifier/number
    /// </summary>
    public ushort InputId { get; internal set; }

    /// <summary>
    /// Long descriptive name for the input
    /// </summary>
    public string LongName { get; internal set; } = string.Empty;

    /// <summary>
    /// Short name for the input
    /// </summary>
    public string ShortName { get; internal set; } = string.Empty;

    /// <summary>
    /// Whether the names are using default values
    /// </summary>
    public bool AreNamesDefault { get; internal set; }

    /// <summary>
    /// Available external port types for this input
    /// </summary>
    public ExternalPortType[]? ExternalPorts { get; internal set; }

    /// <summary>
    /// Current external port type being used
    /// </summary>
    public ExternalPortType ExternalPortType { get; internal set; }

    /// <summary>
    /// Internal port type for this input
    /// </summary>
    public InternalPortType InternalPortType { get; internal set; }

    /// <summary>
    /// Source availability flags indicating where this input can be used
    /// </summary>
    public SourceAvailability SourceAvailability { get; internal set; }

    /// <summary>
    /// Mix effect availability flags indicating which MEs can use this input
    /// </summary>
    public MeAvailability MeAvailability { get; internal set; }

    /// <summary>
    /// Whether this source is live in the program output
    /// </summary>
    public bool IsInProgram { get; internal set; }

    /// <summary>
    /// Whether this source is visible in the preview output
    /// </summary>
    public bool IsInPreview { get; internal set; }
}
