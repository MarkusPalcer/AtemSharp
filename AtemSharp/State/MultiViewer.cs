namespace AtemSharp.State;

/// <summary>
/// MultiViewer configuration and state
/// </summary>
public class MultiViewer
{
	/// <summary>
	/// MultiViewer index
	/// </summary>
	public int Index { get; set; }
    
	/// <summary>
	/// MultiViewer properties (layout, etc.)
	/// </summary>
	public MultiViewerPropertiesState? Properties { get; set; }

	/// <summary>
	/// MultiViewer windows indexed by window index using sparse Dictionary for efficient memory usage
	/// </summary>
	public Dictionary<int, MultiViewerWindowState> Windows { get; set; } = new();

	/// <summary>
	/// VU opacity level (0-100)
	/// </summary>
	public int VuOpacity { get; set; }

	/// <summary>
	/// Create a new MultiViewer with default index
	/// </summary>
	public MultiViewer()
	{
		Index = 0;
	}

	/// <summary>
	/// Create a new MultiViewer with specified index
	/// </summary>
	/// <param name="index">MultiViewer index</param>
	public MultiViewer(int index)
	{
		Index = index;
	}
}