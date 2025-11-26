namespace AtemSharp.State.Settings.MultiViewer;

/// <summary>
/// Properties for MultiViewer configuration
/// </summary>
public class MultiViewerPropertiesState
{
	/// <summary>
	/// MultiViewer layout configuration
	/// </summary>
	public MultiViewerLayout Layout { get; internal set; }

	/// <summary>
	/// Whether program and preview are swapped
	/// </summary>
	public bool ProgramPreviewSwapped { get; internal set; }
}
