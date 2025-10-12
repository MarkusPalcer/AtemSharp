using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Command received from ATEM device containing video mode update
/// </summary>
[Command("VidM")]
public class VideoModeUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Video mode of the device
    /// </summary>
    public VideoMode Mode { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static VideoModeUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mode = reader.ReadByte();

        return new VideoModeUpdateCommand
        {
            Mode = (VideoMode)mode
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
    {
        // Update the video mode
        state.Settings.VideoMode = Mode;

        // Return the state path that was modified
        return ["settings.videoMode"];
    }
}