namespace AtemSharp.State.Video.DownstreamKeyer;

/// <summary>
/// Downstream keyer source configuration
/// </summary>
public class DownstreamKeyerSources
{
	/// <summary>
	/// Fill source input number
	/// </summary>
	public ushort FillSource { get; internal set; }

	/// <summary>
	/// Cut/key source input number
	/// </summary>
	public ushort CutSource { get; internal set; }
}
