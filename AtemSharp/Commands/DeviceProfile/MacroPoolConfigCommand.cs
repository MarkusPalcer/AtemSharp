using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_MAC")]
internal partial class MacroPoolConfigCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _macroCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Info.MacroPool.MacroCount = MacroCount;
        state.Macros.Macros.Populate(MacroCount);
    }
}
