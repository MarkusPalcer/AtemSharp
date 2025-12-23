using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_SSC")]
internal partial class SuperSourceConfigCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _boxCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Info.SuperSources[0].BoxCount = BoxCount;
    }
}
