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
    private bool _nameIsDirty;
    private bool _descriptionIsDirty;

    internal ushort Id => _id;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            Flag |= 1 << 0;
            _nameIsDirty = true;
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            Flag |= 1 << 1;
            _descriptionIsDirty = true;
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

    internal override bool TryMergeTo(SerializedCommand other)
    {
        if (other is not MacroPropertiesCommand target)
        {
            return false;
        }

        if (target._id != _id)
        {
            return false;
        }

        if (_nameIsDirty)
        {
            target.Name = Name;
        }

        if (_descriptionIsDirty)
        {
            target.Description = Description;
        }

        return true;
    }
}
