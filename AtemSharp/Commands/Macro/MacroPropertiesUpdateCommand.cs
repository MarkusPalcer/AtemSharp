using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Macro;

[Command("MPrp")]
public class MacroPropertiesUpdateCommand : IDeserializedCommand
{
    internal MacroPropertiesUpdateCommand()
    {
    }

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        var nameLength = rawCommand.ReadUInt16BigEndian(4);
        var descriptionLength = rawCommand.ReadUInt16BigEndian(6);

        return new MacroPropertiesUpdateCommand
        {
            Id = rawCommand.ReadUInt16BigEndian(0),
            IsUsed = rawCommand.ReadBoolean(2),
            HasUnsupportedOps = rawCommand.ReadBoolean(3),
            Name = nameLength > 0 ? rawCommand.ReadString(8, nameLength) : string.Empty,
            Description = descriptionLength > 0 ? rawCommand.ReadString(8 + nameLength, descriptionLength) : string.Empty,
        };
    }

    public required string Description { get; init; }

    public required string Name { get; init; }

    public bool HasUnsupportedOps { get; init; }

    public bool IsUsed { get; init; }

    public ushort Id { get; init; }

    public void ApplyToState(AtemState state)
    {
        if (Id >= state.Macros.Macros.Length)
        {
            throw new IndexOutOfRangeException($"Macro ID {Id} is out of range of the macro state array which has length {state.Macros.Macros.Length}");
        }

        var macro = state.Macros.Macros[Id];
        macro.Name = Name;
        macro.Description = Description;
        macro.IsUsed = IsUsed;
        macro.HasUnsupportedOps = HasUnsupportedOps;
    }
}
