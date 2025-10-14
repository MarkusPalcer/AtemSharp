using AtemSharp.Enums;
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
    /// <summary>
    /// Power supply status array. Each element represents the status of a power supply.
    /// true = power supply is working, false = power supply has failed or is not present.
    /// </summary>
    public bool[] PowerSupplies { get; set; } = [];

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static PowerStatusCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        // Read the power status byte
        var powerStatusByte = reader.ReadByte();

        // Extract individual power supply status bits
        // Bit 0 = first power supply, Bit 1 = second power supply
        var powerSupplies = new[]
        {
            (powerStatusByte & (1 << 0)) != 0,  // First power supply
            (powerStatusByte & (1 << 1)) != 0   // Second power supply
        };

        return new PowerStatusCommand
        {
            PowerSupplies = powerSupplies
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
