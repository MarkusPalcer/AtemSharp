using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Commands.Macro;

[Command("MRCP")]
public class MacroRunStatusCommand : SerializedCommand
{
    // Change flags not needed - only one property. No change = no command
    public bool Loop { get; set; }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[4];
        // Flag: Always write loop - it does not make sense to send the command when no change is made
        buffer.WriteUInt8(1, 0);
        buffer.WriteBoolean(Loop, 1);
        return buffer;
    }
}
