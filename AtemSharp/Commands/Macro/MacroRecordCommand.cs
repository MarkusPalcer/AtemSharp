using AtemSharp.State.Info;

namespace AtemSharp.Commands.Macro;

/// <summary>
/// Used to start recording a macro
/// </summary>
// Manual serialization due to variable buffer size
[Command("MSRc")]
public class MacroRecordCommand(AtemSharp.State.Macro.Macro targetSlot) : SerializedCommand
{
    internal ushort Index = targetSlot.Id;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            _nameIsDirty = true;
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            _descriptionIsDirty = true;
        }
    }

    private bool _nameIsDirty;
    private bool _descriptionIsDirty;
    private string _name = targetSlot.Name;
    private string _description = targetSlot.Description;

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[SerializationExtensions.PadToMultiple(8 + Name.Length + Description.Length, 4)];
        buffer.WriteUInt16BigEndian(Index, 0);
        buffer.WriteUInt16BigEndian((ushort)Name.Length, 2);
        buffer.WriteUInt16BigEndian((ushort)Description.Length, 4);
        buffer.WriteString(Name, 6);
        buffer.WriteString(Description, 6 + Name.Length);

        return buffer;
    }

    internal override bool TryMergeTo(SerializedCommand other)
    {
        if (other is not MacroRecordCommand target)
        {
            return false;
        }

        // We can only record one macro, so if a new record is queued it replaces the old record
        if (target.Index != Index)
        {
            target.Index = Index;
            target.Name = Name;
            target.Description = Description;

            return true;
        }

        // Else only replace properties which have values that are changed

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
