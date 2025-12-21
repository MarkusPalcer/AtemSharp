using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// SuperSource configuration command received from ATEM
/// </summary>
/// <remarks>
/// Used for protocol versions 8.0 and higher
/// </remarks>
[Command("_SSC", ProtocolVersion.V8_0)]
public partial class SuperSourceConfigCommandV8 : IDeserializedCommand
{
    [DeserializedField(0)] private byte _superSourceId;
    [DeserializedField(2)] private byte _boxCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Info.SuperSources[SuperSourceId].BoxCount = BoxCount;
    }
}
