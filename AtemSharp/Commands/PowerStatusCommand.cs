using AtemSharp.State;

namespace AtemSharp.Commands;

[Command("Powr")]
internal partial class PowerStatusCommand : IDeserializedCommand
{
    [DeserializedField(0)] [NoProperty] private PowerSupplyBits _powerStatus;
    [CustomDeserialization] private bool[] _powerSupplies = [];

    [Flags]
    private enum PowerSupplyBits : byte
    {
        First = 1,
        Second = 2
    }

    private void DeserializeInternal(ReadOnlySpan<byte> _)
    {

        _powerSupplies =
        [
            _powerStatus.HasFlag(PowerSupplyBits.First),
            _powerStatus.HasFlag(PowerSupplyBits.Second),
        ];
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        for (var i = 0; i < state.Info.Power.Length; i++)
        {
            state.Info.Power[i] = _powerSupplies[i];
        }
    }
}
