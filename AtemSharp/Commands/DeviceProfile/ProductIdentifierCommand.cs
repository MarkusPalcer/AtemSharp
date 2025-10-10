using System.Text;
using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Product identifier command received from ATEM
/// </summary>
[Command("_pin")]
public class ProductIdentifierCommand : IDeserializedCommand
{
    /// <summary>
    /// Product identifier string from the device
    /// </summary>
    public string ProductIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// ATEM device model
    /// </summary>
    public Model Model { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static ProductIdentifierCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        // Read 40 bytes for product identifier and extract null-terminated string
        var productIdentifierBytes = reader.ReadBytes(40);
        var productIdentifier = AtemUtil.BufferToNullTerminatedString(productIdentifierBytes, 0, 40);

        // Read model as single byte
        var model = (Model)reader.ReadByte();

        return new ProductIdentifierCommand
        {
            ProductIdentifier = productIdentifier,
            Model = model
        };
    }

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Paths indicating what was changed in the state</returns>
    public string[] ApplyToState(AtemState state)
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

        return ["info"];
    }
}