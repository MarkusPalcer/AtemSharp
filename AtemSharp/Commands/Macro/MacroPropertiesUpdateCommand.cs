using AtemSharp.State;

namespace AtemSharp.Commands.Macro;

[Command("MPrp")]
public partial class MacroPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _id;

    [DeserializedField(2)] private bool _isUsed;

    [DeserializedField(3)] private bool _hasUnsupportedOps;

    // Stryker disable once string : initialization is always overriden by deserialization
    [CustomDeserialization] private string _description = string.Empty;

    // Stryker disable once string : initialization is always overriden by deserialization
    [CustomDeserialization] private string _name = string.Empty;

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand)
    {
        var nameLength = rawCommand.ReadUInt16BigEndian(4);
        var descriptionLength = rawCommand.ReadUInt16BigEndian(6);

        Name = rawCommand.ReadString(8, nameLength);
        Description = rawCommand.ReadString(8 + nameLength, descriptionLength);
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var macro = state.Macros.Macros[Id];
        macro.Name = Name;
        macro.Description = Description;
        macro.IsUsed = IsUsed;
        macro.HasUnsupportedOps = HasUnsupportedOps;
    }
}
