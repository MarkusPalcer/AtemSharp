using AtemSharp.State;

namespace AtemSharp.Commands;

/// <summary>
/// Command containing power status information from the ATEM device.
/// This command provides the power supply status for each power supply in the device.
/// As defined in DeviceProfile/ProductIdentifierCommand.cs, the 2ME, 2ME 4K and the
/// Broadcast Studio have 2 power supplies. All other models have 1.
/// </summary>
[Command("Powr")]
public partial class PowerStatusCommand : IDeserializedCommand
{
    [DeserializedField(0)] [NoProperty] private PowerSupplyBits _powerStatus;

    /// <summary>
    /// Power supply status array. Each element represents the status of a power supply.
    /// true = power supply is working, false = power supply has failed or is not present.
    /// </summary>
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
