using System.Text;
using AtemSharp.Enums;
using AtemSharp.State;

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
    public SupportedVideoMode[] SupportedVideoModes { get; set; } = [];

    public static VideoMixerConfigCommand Deserialize(Stream stream, ProtocolVersion version)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        var hasRequiresReconfig = version >= ProtocolVersion.V8_0;
        var size = hasRequiresReconfig ? 13 : 12;

        var count = reader.ReadUInt16BigEndian();
        reader.ReadBytes(2); // Skip padding

        var modes = new SupportedVideoMode[count];
        
        for (int i = 0; i < count; i++)
        {
            var baseOffset = i * size;
            
            // Seek to the correct position for this mode
            stream.Seek(4 + baseOffset, SeekOrigin.Begin);
            
            var mode = reader.ReadByte();
            reader.ReadBytes(3); // Skip padding
            
            var multiviewerModeMask = reader.ReadUInt32BigEndian();
            var downConvertModeMask = reader.ReadUInt32BigEndian();
            
            var requiresReconfig = false;
            if (hasRequiresReconfig)
            {
                requiresReconfig = reader.ReadBoolean();
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
    public string[] ApplyToState(AtemState state)
    {
        state.Info.SupportedVideoModes = SupportedVideoModes;
        return ["info.supportedVideoModes"];
    }
}