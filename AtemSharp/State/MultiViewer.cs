namespace AtemSharp.State;

/// <summary>
/// MultiViewer configuration and state
/// </summary>
public class MultiViewer : ArrayItem
{
    internal override void SetId(int id) => Id = (byte)id;

    /// <summary>
    /// MultiViewer index
    /// </summary>
    public byte Id { get; internal set; }

    /// <summary>
    /// MultiViewer properties (layout, etc.)
    /// </summary>
    public MultiViewerPropertiesState Properties { get; } = new();

    /// <summary>
    /// MultiViewer windows indexed by window index using sparse Dictionary for efficient memory usage
    /// </summary>
    public Dictionary<int, MultiViewerWindowState> Windows { get; } = new();

    /// <summary>
    /// VU opacity level (0-100)
    /// </summary>
    public byte VuOpacity { get; internal set; }
}
