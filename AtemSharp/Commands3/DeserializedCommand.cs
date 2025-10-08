using AtemSharp.State;

namespace AtemSharp.Commands3;

public interface IDeserializedCommand
{
	/// <summary>
	/// Apply this command to the ATEM state
	/// </summary>
	/// <param name="state">ATEM state to modify</param>
	/// <returns>List of state paths that were changed</returns>
	string[] ApplyToState(AtemState state);
}
