using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Version command received from ATEM device
/// </summary>
[Command("_ver")]
public partial class VersionCommand : IDeserializedCommand
{
    /// <summary>
    /// ATEM protocol version
    /// </summary>
    [DeserializedField(0)]
    private ProtocolVersion _version;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update the API version in device info
        state.Info.ApiVersion = Version;
    }
}
