namespace AtemSharp.State;

/// <summary>
/// Utility methods for working with ATEM state
/// </summary>
public static class AtemStateUtil
{
    /// <summary>
    /// Get or create a mix effect at the specified index
    /// </summary>
    /// <param name="state">ATEM state</param>
    /// <param name="index">Mix effect index (0-based)</param>
    /// <returns>The mix effect at the specified index</returns>
    public static MixEffect GetMixEffect(AtemState state, int index)
    {
        // Ensure Video state exists
        state.Video ??= new VideoState();
        
        // Ensure MixEffects array is large enough
        if (state.Video.MixEffects.Length <= index)
        {
            var newArray = new MixEffect?[index + 1];
            Array.Copy(state.Video.MixEffects, newArray, state.Video.MixEffects.Length);
            state.Video.MixEffects = newArray;
        }

        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects[index];
        if (mixEffect != null) return mixEffect!;
        
        mixEffect = new MixEffect
        {
	        Index = index,
	        ProgramInput = 0,
	        PreviewInput = 0,
	        TransitionPreview = false,
	        TransitionPosition = new TransitionPosition
	        {
		        InTransition = false,
		        HandlePosition = 0,
		        RemainingFrames = 0
	        },
	        TransitionProperties = new TransitionProperties(),
	        TransitionSettings = new TransitionSettings(),
	        UpstreamKeyers = []
        };
            
        state.Video.MixEffects[index] = mixEffect;

        return mixEffect;
    }
}