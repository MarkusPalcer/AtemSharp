using AtemSharp.State;

namespace AtemSharp.Commands;

public abstract class DeserializedCommand : ICommand
{
	/// <summary>
	/// Apply this command to the ATEM state
	/// </summary>
	/// <param name="state">ATEM state to modify</param>
	/// <returns>List of state paths that were changed</returns>
	public abstract string[] ApplyToState(AtemState state);
}
