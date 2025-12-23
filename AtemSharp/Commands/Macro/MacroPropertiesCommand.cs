using AtemSharp.State.Info;

namespace AtemSharp.Commands.Macro;

/// <summary>
/// Used to change the name and description of a macro after recording it
/// </summary>
// This class needs to be serialized manually, because the buffer size is
// dynamic which is not supported by code generation
[Command("CMPr")]
public class MacroPropertiesCommand(State.Macro.Macro macro) : SerializedCommand
{
    private readonly ushort _id = macro.Id;
    private string _name = macro.Name;
    private string _description = macro.Description;

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

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[SerializationExtensions.PadToMultiple(8 + _name.Length + _description.Length, 4)];
        buffer.WriteUInt8((byte)Flag, 0);
        buffer.WriteUInt16BigEndian(_id, 2);
        buffer.WriteUInt16BigEndian((ushort)_name.Length, 4);
        buffer.WriteUInt16BigEndian((ushort)_description.Length, 6);
        buffer.WriteString(_name, 8);
        buffer.WriteString(_description, 8 + _name.Length);

        return buffer;
    }
}
