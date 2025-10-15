using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Media pool configuration command received from ATEM
/// </summary>
[Command("_mpl")]
public class MediaPoolConfigCommand : IDeserializedCommand
{
    /// <summary>
    /// Number of still images available in the media pool
    /// </summary>
    public byte StillCount { get; init; }

    /// <summary>
    /// Number of video clips available in the media pool
    /// </summary>
    public byte ClipCount { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static MediaPoolConfigCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new MediaPoolConfigCommand
        {
            StillCount = rawCommand.ReadUInt8(0),
            ClipCount = rawCommand.ReadUInt8(1)
        };
    }

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

        state.Media.Frames.ForEachWithIndex((item, index) => item.Id = (byte)index);
        state.Media.Clips.ForEachWithIndex((item, index) => item.Id = (byte)index);
    }
}
