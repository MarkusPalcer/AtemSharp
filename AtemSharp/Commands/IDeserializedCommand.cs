using AtemSharp.State;

namespace AtemSharp.Commands;

/// <summary>
/// A command sent from the ATEM device
/// </summary>
public interface IDeserializedCommand : ICommand
{
    /// <summary>
    /// Apply this command to the ATEM state
    /// </summary>
    /// <param name="state">ATEM state to modify</param>
    public void ApplyToState(AtemState state);

    public void Apply(IStateHolder switcher)
    {
        ApplyToState(switcher.State);
    }
}
