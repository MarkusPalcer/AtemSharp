using System.Text;
using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Macro pool configuration command received from ATEM
/// </summary>
[Command("_MAC")]
public class MacroPoolConfigCommand : IDeserializedCommand
{
    /// <summary>
    /// Number of macros available in the macro pool
    /// </summary>
    public byte MacroCount { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <returns>Deserialized command instance</returns>
    public static MacroPoolConfigCommand Deserialize(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        var macroCount = reader.ReadByte();

        return new MacroPoolConfigCommand
        {
            MacroCount = macroCount
        };
    }

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Paths indicating what was changed in the state</returns>
    public string[] ApplyToState(AtemState state)
    {
        // Update device info macro pool configuration
        state.Info.MacroPool = new MacroPoolInfo
        {
            MacroCount = MacroCount
        };

        return ["info.macroPool"];
    }
}