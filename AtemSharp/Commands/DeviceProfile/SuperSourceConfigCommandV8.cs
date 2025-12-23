using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_SSC", ProtocolVersion.V8_0)]
internal partial class SuperSourceConfigCommandV8 : IDeserializedCommand
{
    [DeserializedField(0)] private byte _superSourceId;
    [DeserializedField(2)] private byte _boxCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Info.SuperSources[SuperSourceId].BoxCount = BoxCount;
    }
}
