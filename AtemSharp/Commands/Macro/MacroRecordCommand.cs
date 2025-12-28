using AtemSharp.State.Info;

namespace AtemSharp.Commands.Macro;

/// <summary>
/// Used to start recording a macro
/// </summary>
// Manual serialization due to variable buffer size
[Command("MSRc")]
public class MacroRecordCommand(AtemSharp.State.Macro.Macro targetSlot) : SerializedCommand
{
    internal readonly ushort Index = targetSlot.Id;

    public string Name { get; set; } = targetSlot.Name;
    public string Description { get; set; } = targetSlot.Description;

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
}
