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
    [DeserializedField(0)]
    private byte _macroCount;

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Paths indicating what was changed in the state</returns>
    public void ApplyToState(AtemState state)
    {
        // Update device info macro pool configuration
        state.Info.MacroPool = new MacroPoolInfo
        {
            MacroCount = MacroCount
        };

        state.Macros.Macros = AtemStateUtil.CreateArray<State.Macro>(MacroCount).ForEachWithIndex((macro, i) => macro.Id = (ushort)i);
    }
}
