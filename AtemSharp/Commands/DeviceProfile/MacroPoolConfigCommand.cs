using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Macro pool configuration command received from ATEM
/// </summary>
[Command("_MAC")]
public partial class MacroPoolConfigCommand : IDeserializedCommand
{
    /// <summary>
    /// Number of macros available in the macro pool
    /// </summary>
    [DeserializedField(0)] private byte _macroCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Info.MacroPool.MacroCount = MacroCount;
        state.Macros.Macros.Populate(MacroCount);
    }
}
