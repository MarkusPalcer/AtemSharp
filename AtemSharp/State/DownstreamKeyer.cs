namespace AtemSharp.State;

/// <summary>
/// Downstream keyer state
/// </summary>
public class DownstreamKeyer : IDownstreamKeyerBase
{
    /// <inheritdoc />
    public bool InTransition { get; set; }

    /// <inheritdoc />
    public int RemainingFrames { get; set; }

    /// <inheritdoc />
    public bool IsAuto { get; set; }

    /// <inheritdoc />
    public bool OnAir { get; set; }

    /// <inheritdoc />
    public bool? IsTowardsOnAir { get; set; }

    /// <summary>
    /// Downstream keyer source configuration
    /// </summary>
    public DownstreamKeyerSources? Sources { get; set; }

    /// <summary>
    /// Downstream keyer properties and configuration
    /// </summary>
    public DownstreamKeyerProperties? Properties { get; set; }
}