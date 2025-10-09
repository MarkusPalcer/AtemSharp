namespace AtemSharp.State;

/// <summary>
/// Video state container
/// </summary>
public class VideoState
{
	public Dictionary<int, MixEffectState> MixEffects { get; set; } = new();
    
	// TODO: Add more video state properties
}