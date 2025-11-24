using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Commands.Macro;

// Manual serialization due to variable buffer size
[Command("MSRc")]
public class MacroRecordCommand(AtemSharp.State.Macro targetSlot) : SerializedCommand
{
    private ushort _index = targetSlot.Id;

    public string Name { get; set; } = targetSlot.Name;
    public string Description { get; set; } = targetSlot.Description;

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[AtemUtil.PadToMultiple(8 + Name.Length + Description.Length, 4)];
        buffer.WriteUInt16BigEndian(_index, 0);
        buffer.WriteUInt16BigEndian((ushort)Name.Length, 2);
        buffer.WriteUInt16BigEndian((ushort)Description.Length, 4);
        buffer.WriteString(Name, 6);
        buffer.WriteString(Description, 6 + Name.Length);

        return buffer;
    }
}
