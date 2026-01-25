using AtemSharp.State.Info;

namespace AtemSharp.Commands.Macro;

/// <summary>
/// Used to change the name and description of a macro after recording it
/// </summary>
/// <remarks>
/// ATEM Control Software updates name and description by sending one command for each,
/// but I verified that you can send it with both at the same time, too.
/// </remarks>
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
        var nameLength = _nameIsDirty ? _name.Length : 0;
        var descriptionLength = _descriptionIsDirty ? _description.Length : 0;

        var buffer = new byte[SerializationExtensions.PadToMultiple(8 + nameLength + descriptionLength, 4)];
        buffer.WriteUInt8((byte)Flag, 0);
        buffer.WriteUInt16BigEndian(_id, 2);
        buffer.WriteUInt16BigEndian((ushort)nameLength, 4);
        buffer.WriteUInt16BigEndian((ushort)descriptionLength, 6);

        if (_nameIsDirty)
        {
            buffer.WriteString(_name, 8);
        }

        if (_descriptionIsDirty)
        {
            buffer.WriteString(_description, 8 + nameLength);
        }

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
