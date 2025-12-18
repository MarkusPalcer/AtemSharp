using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Settings;

/// <summary>
/// Settings state container for ATEM devices
/// </summary>
public class SettingsState
{
    internal SettingsState()
    {
        MultiViewers = new ItemCollection<byte, MultiViewer.MultiViewer>(id => new MultiViewer.MultiViewer { Id = id });
    }

    /// <summary>
    /// Current video mode of the device
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public VideoMode VideoMode { get; internal set; }

    /// <summary>
    /// Current time mode of the device
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public TimeMode TimeMode { get; internal set; }

    /// <summary>
    /// MultiViewer settings indexed by MultiViewer ID using sparse Dictionary for efficient memory usage
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<byte, MultiViewer.MultiViewer> MultiViewers { get; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public MediaPoolSettings MediaPool { get; } = new();
}
