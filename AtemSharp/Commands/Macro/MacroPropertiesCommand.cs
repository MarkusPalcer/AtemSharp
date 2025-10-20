using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Commands.Macro;

[Command("CMPr")]
public class MacroPropertiesCommand : SerializedCommand
{
    private readonly ushort _id;
    private string _name;
    private string _description;

    public MacroPropertiesCommand(State.Macro macro)
    {
        _id = macro.Id;
        _name = macro.Name;
        _description = macro.Description;
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            Flag |= 1 << 0;
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            Flag |= 1 << 1;
        }
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[AtemUtil.PadToMultiple(8+ _name.Length + _description.Length, 4)];
        buffer.WriteUInt8((byte)Flag,0);
        buffer.WriteUInt16BigEndian(_id, 2);
        buffer.WriteUInt16BigEndian(_name.Length, 4);
        buffer.WriteUInt16BigEndian(_description.Length, 6);
        buffer.WriteString(_name, 8);
        buffer.WriteString(_description, 8 + _name.Length);

        return buffer;
    }
}
