using AtemSharp.State;
using AtemSharp.State.Info;
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

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Paths indicating what was changed in the state</returns>
    public void ApplyToState(AtemState state)
    {
        // Update device info media pool configuration
        state.Info.MediaPool = new MediaPoolInfo
        {
            StillCount = StillCount,
            ClipCount = ClipCount
        };

        state.Media.Frames = AtemStateUtil.CreateArray<MediaPoolEntry>(StillCount);
        state.Media.Clips = AtemStateUtil.CreateArray<MediaPoolEntry>(ClipCount);
    }
}
