using AtemSharp.State;
using AtemSharp.State.Media;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Media pool configuration command received from ATEM
/// </summary>
[Command("_mpl")]
public partial class MediaPoolConfigCommand : IDeserializedCommand
{
    /// <summary>
    /// Number of still images available in the media pool
    /// </summary>
    [DeserializedField(0)] private byte _stillCount;

    /// <summary>
    /// Number of video clips available in the media pool
    /// </summary>
    [DeserializedField(1)] private byte _clipCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update device info media pool configuration
        state.Info.MediaPool.StillCount = StillCount;
        state.Info.MediaPool.ClipCount = ClipCount;

        state.Media.Frames = AtemStateUtil.CreateArray<MediaPoolEntry>(StillCount);
        state.Media.Clips = AtemStateUtil.CreateArray<MediaPoolEntry>(ClipCount);
    }
}
