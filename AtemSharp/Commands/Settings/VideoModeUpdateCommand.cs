using AtemSharp.Enums;
using AtemSharp.Lib;
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
    public VideoMode Mode { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static VideoModeUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new VideoModeUpdateCommand
        {
            Mode = (VideoMode)rawCommand.ReadUInt8(0)
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update the video mode
        state.Settings.VideoMode = Mode;
    }
}
