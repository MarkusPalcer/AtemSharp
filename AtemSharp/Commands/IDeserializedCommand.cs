using AtemSharp.State;

namespace AtemSharp.Commands;

public interface IDeserializedCommand : ICommand
{
	/// <summary>
	/// Apply this command to the ATEM state
	/// </summary>
	/// <param name="state">ATEM state to modify</param>
	public void ApplyToState(AtemState state);
}
