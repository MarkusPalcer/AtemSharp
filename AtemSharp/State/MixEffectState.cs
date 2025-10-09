using AtemSharp.Enums;

namespace AtemSharp.State;

/// <summary>
/// Mix Effect state
/// </summary>
public class MixEffectState
{
	public int? ProgramInput { get; set; }
	public int? PreviewInput { get; set; }
	public bool InTransition { get; set; }
	public TransitionStyle TransitionStyle { get; set; }
	public TransitionSelection TransitionSelection { get; set; }
    
	// TODO: Add more MixEffect properties as needed
}