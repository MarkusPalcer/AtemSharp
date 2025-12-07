using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Product identifier command received from ATEM
/// </summary>
[Command("_pin")]
public partial class ProductIdentifierCommand : IDeserializedCommand
{
    /// <summary>
    /// ATEM device model
    /// </summary>
    [DeserializedField(40)] private Model _model;

    // Stryker disable once string : initialization is always overriden by deserialization
    /// <summary>
    /// Product identifier string from the device
    /// </summary>
    public string ProductIdentifier { get; internal set; } = string.Empty;

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand)
    {
        ProductIdentifier = rawCommand.ReadString(0, 40);
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update device info
        state.Info.ProductIdentifier = ProductIdentifier;
        state.Info.Model = Model;

        // Model specific features that aren't specified by the protocol
        // Initialize power supply status based on device model
        switch (Model)
        {
            case Model.TwoME:
            case Model.TwoME4K:
            case Model.TwoMEBS4K:
            case Model.Constellation:
            case Model.Constellation8K:
            case Model.ConstellationHD4ME:
            case Model.Constellation4K4ME:
                state.Info.Power = [false, false];
                break;
            default:
                state.Info.Power = [false];
                break;
        }
    }
}
