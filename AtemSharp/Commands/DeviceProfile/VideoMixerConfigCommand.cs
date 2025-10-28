using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Command to receive video mixer configuration information
/// </summary>
[Command("_VMC")]
public class VideoMixerConfigCommand : IDeserializedCommand
{
    /// <summary>
    /// Array of supported video modes
    /// </summary>
    public SupportedVideoMode[] SupportedVideoModes { get; init; } = [];

    // TODO: Split by Version
    public static VideoMixerConfigCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        var hasRequiresReconfig = version >= ProtocolVersion.V8_0;
        var size = hasRequiresReconfig ? 13 : 12;

        var count = rawCommand.ReadUInt16BigEndian(0);

        var modes = new SupportedVideoMode[count];

        for (int i = 0; i < count; i++)
        {
            var baseOffset = 4 + (i * size); // Start after the count (4 bytes) plus mode data

            var mode = rawCommand.ReadUInt8(baseOffset);

            var multiviewerModeMask = rawCommand.ReadUInt32BigEndian(baseOffset + 4);
            var downConvertModeMask = rawCommand.ReadUInt32BigEndian(baseOffset + 8);

            var requiresReconfig = false;
            if (hasRequiresReconfig)
            {
                requiresReconfig = rawCommand.ReadBoolean(baseOffset + 12);
            }

            modes[i] = new SupportedVideoMode
            {
                Mode = (VideoMode)mode,
                MultiviewerModes = ReadVideoModeBitmask(multiviewerModeMask),
                DownConvertModes = ReadVideoModeBitmask(downConvertModeMask),
                RequiresReconfig = requiresReconfig
            };
        }

        return new VideoMixerConfigCommand
        {
            SupportedVideoModes = modes
        };
    }

    /// <summary>
    /// Read a bitmask and convert it to an array of VideoMode values
    /// </summary>
    /// <param name="rawVal">The bitmask value</param>
    /// <returns>Array of video modes</returns>
    private static VideoMode[] ReadVideoModeBitmask(uint rawVal)
    {
        var modes = new List<VideoMode>();

        // Check each possible VideoMode enum value
        foreach (VideoMode possibleMode in Enum.GetValues<VideoMode>())
        {
            if ((rawVal & (1u << (int)possibleMode)) != 0)
            {
                modes.Add(possibleMode);
            }
        }

        return modes.ToArray();
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Info.SupportedVideoModes = SupportedVideoModes;
    }
}
