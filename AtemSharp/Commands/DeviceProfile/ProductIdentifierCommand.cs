using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State;

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
    [DeserializedField(40)]
    private Model _model;

    /// <summary>
    /// Product identifier string from the device
    /// </summary>
    public string ProductIdentifier { get; internal set; } = string.Empty;

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        ProductIdentifier = rawCommand.ReadString(0, 40);
    }

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Paths indicating what was changed in the state</returns>
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
