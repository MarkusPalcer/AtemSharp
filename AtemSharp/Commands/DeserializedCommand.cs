using AtemSharp.State;

namespace AtemSharp.Commands;

public interface IDeserializedCommand : ICommand
{
	/// <summary>
	/// Apply this command to the ATEM state
	/// </summary>
	/// <param name="state">ATEM state to modify</param>
	/// <returns>List of state paths that were changed</returns>
	public string[] ApplyToState(AtemState state);
}
