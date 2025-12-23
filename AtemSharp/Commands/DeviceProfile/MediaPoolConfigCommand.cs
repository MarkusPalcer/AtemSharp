using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_mpl")]
internal partial class MediaPoolConfigCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _stillCount;
    [DeserializedField(1)] private byte _clipCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update device info media pool configuration
        state.Info.MediaPool.StillCount = StillCount;
        state.Info.MediaPool.ClipCount = ClipCount;

        state.Media.Frames.Populate(StillCount);
        state.Media.Clips.Populate(ClipCount);
    }
}
