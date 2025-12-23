using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_ver")]
internal partial class VersionCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ProtocolVersion _version;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update the API version in device info
        state.Info.ApiVersion = Version;
    }
}
