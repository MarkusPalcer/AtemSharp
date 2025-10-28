using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands;

/// <summary>
/// Command containing power status information from the ATEM device.
/// This command provides the power supply status for each power supply in the device.
/// As defined in DeviceProfile/ProductIdentifierCommand.cs, the 2ME, 2ME 4K and the
/// Broadcast Studio have 2 power supplies. All other models have 1.
/// </summary>
[Command("Powr")]
public class PowerStatusCommand : IDeserializedCommand
{
    // Command is manually deserialized because the only field it has needs to be manually deserialized

    /// <summary>
    /// Power supply status array. Each element represents the status of a power supply.
    /// true = power supply is working, false = power supply has failed or is not present.
    /// </summary>
    public bool[] PowerSupplies { get; init; } = [];

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static PowerStatusCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        // Read the power status byte
        var powerStatusByte = rawCommand.ReadUInt8(0);

        return new PowerStatusCommand
        {
            PowerSupplies =
            [
                // Extract individual power supply status bits
                // Bit 0 = first power supply, Bit 1 = second power supply
                (powerStatusByte & (1 << 0)) != 0,  // First power supply
                (powerStatusByte & (1 << 1)) != 0   // Second power supply
            ]
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Only update the number of power supplies that are configured for this device
        var configuredSupplyCount = state.Info.Power.Length;

        // Take only the configured number of power supplies from our data
        // This prevents overwriting with more power supplies than the device actually has
        state.Info.Power = PowerSupplies.Take(configuredSupplyCount).ToArray();
    }
}
