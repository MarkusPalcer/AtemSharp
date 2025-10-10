using System.Text;
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
    public byte StillCount { get; set; }

    /// <summary>
    /// Number of video clips available in the media pool
    /// </summary>
    public byte ClipCount { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <returns>Deserialized command instance</returns>
    public static MediaPoolConfigCommand Deserialize(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        var stillCount = reader.ReadByte();
        var clipCount = reader.ReadByte();

        return new MediaPoolConfigCommand
        {
            StillCount = stillCount,
            ClipCount = clipCount
        };
    }

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Paths indicating what was changed in the state</returns>
    public string[] ApplyToState(AtemState state)
    {
        // Update device info media pool configuration
        state.Info.MediaPool = new MediaPoolInfo
        {
            StillCount = StillCount,
            ClipCount = ClipCount
        };

        return ["info.mediaPool"];
    }
}