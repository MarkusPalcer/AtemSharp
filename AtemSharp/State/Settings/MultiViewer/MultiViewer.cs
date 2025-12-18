using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Settings.MultiViewer;

/// <summary>
/// MultiViewer configuration and state
/// </summary>
public class MultiViewer
{
    public MultiViewer()
    {
        Windows = new ItemCollection<byte, MultiViewerWindowState>(id => new MultiViewerWindowState
        {
            WindowIndex = id,
            MultiViewerId = Id
        });
    }

    /// <summary>
    /// MultiViewer index
    /// </summary>
    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public byte Id { get; internal set; }

    /// <summary>
    /// MultiViewer properties (layout, etc.)
    /// </summary>
    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public MultiViewerPropertiesState Properties { get; } = new();

    /// <summary>
    /// MultiViewer windows indexed by window index using sparse Dictionary for efficient memory usage
    /// </summary>
    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public ItemCollection<byte, MultiViewerWindowState> Windows { get; }

    /// <summary>
    /// VU opacity level (0-100)
    /// </summary>
    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public byte VuOpacity { get; internal set; }
}
