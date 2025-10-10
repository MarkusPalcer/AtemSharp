namespace AtemSharp.State;

/// <summary>
/// Downstream keyer source configuration
/// </summary>
public class DownstreamKeyerSources
{
	/// <summary>
	/// Fill source input number
	/// </summary>
	public int FillSource { get; set; }

	/// <summary>
	/// Cut/key source input number
	/// </summary>
	public int CutSource { get; set; }
}