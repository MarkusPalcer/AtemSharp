namespace AtemSharp.State.Video.DownstreamKeyer;

/// <summary>
/// Downstream keyer state
/// </summary>
public class DownstreamKeyer : ArrayItem
{
    internal override void SetId(int id) => Id = (byte)id;
    public byte Id { get; internal set; }

    /// <summary>
    /// Whether the downstream keyer is currently in transition
    /// </summary>

    public bool InTransition { get; internal set; }

    /// <summary>
    /// Number of frames remaining in the current transition
    /// </summary>

    public int RemainingFrames { get; internal set; }

    /// <summary>
    /// Whether the downstream keyer is in auto transition mode
    /// </summary>

    public bool IsAuto { get; internal set; }

    /// <summary>
    /// Whether the downstream keyer is currently on air
    /// </summary>
    public bool OnAir { get; internal set; }

    /// <summary>
    /// Direction of the auto transition (true = towards on air, false = towards off air)
    /// </summary>
    public bool IsTowardsOnAir { get; internal set; }

    /// <summary>
    /// Downstream keyer source configuration
    /// </summary>
    public DownstreamKeyerSources Sources { get; internal set; } = new();

    /// <summary>
    /// Downstream keyer properties and configuration
    /// </summary>
    public DownstreamKeyerProperties Properties { get; internal set; } = new();
}
