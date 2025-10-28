using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Macro;

[Command("MPrp")]
public partial class MacroPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private ushort _id;

    [DeserializedField(2)]
    private bool _isUsed;

    [DeserializedField(3)]
    private bool _hasUnsupportedOps;

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        var nameLength = rawCommand.ReadUInt16BigEndian(4);
        var descriptionLength = rawCommand.ReadUInt16BigEndian(6);

        Name = nameLength > 0 ? rawCommand.ReadString(8, nameLength) : string.Empty;
        Description = descriptionLength > 0 ? rawCommand.ReadString(8 + nameLength, descriptionLength) : string.Empty;
    }

    public string Description { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

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
