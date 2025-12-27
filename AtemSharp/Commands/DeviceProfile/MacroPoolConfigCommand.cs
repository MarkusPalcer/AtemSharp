using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_MAC")]
internal partial class MacroPoolConfigCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _macroCount;

    /// <inheritdoc />
    public void Apply(IStateHolder stateHolder)
    {
        stateHolder.State.Info.MacroPool.MacroCount = MacroCount;
        stateHolder.Macros.Populate(MacroCount);
    }

    public void ApplyToState(AtemState state)
    {

    }
}
