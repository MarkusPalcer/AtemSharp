using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.State.Settings;

namespace AtemSharp.Commands.DeviceProfile;

public class VideoMixerConfigCommandBase : IDeserializedCommand
{
    /// <summary>
    /// Array of supported video modes
    /// </summary>
    public SupportedVideoMode[] SupportedVideoModes { get; protected set; } = [];

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        if (state.Info.SupportedVideoModes.Length != SupportedVideoModes.Length)
        {
            state.Info.SupportedVideoModes = SupportedVideoModes;
            return;
        }

        foreach (var (stateEntry, newEntry) in state.Info.SupportedVideoModes.Zip(SupportedVideoModes))
        {
            stateEntry.DownConvertModes = newEntry.DownConvertModes;
            stateEntry.MultiviewerModes = newEntry.MultiviewerModes;
            stateEntry.Mode = newEntry.Mode;
            stateEntry.RequiresReconfiguration = newEntry.RequiresReconfiguration;
        }
    }

    protected SupportedVideoMode ParseVideoMode(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        return new SupportedVideoMode
        {
            Mode = (VideoMode)rawCommand.ReadUInt8(0),
            MultiviewerModes = ReadVideoModeBitmask(rawCommand.ReadUInt32BigEndian(4)),
            DownConvertModes = ReadVideoModeBitmask(rawCommand.ReadUInt32BigEndian(8)),
            RequiresReconfiguration = version >= ProtocolVersion.V8_0 && rawCommand.ReadBoolean(12)
        };
    }

    /// <summary>
    /// Read a bitmask and convert it to an array of VideoMode values
    /// </summary>
    /// <param name="rawVal">The bitmask value</param>
    /// <returns>Array of video modes</returns>
    protected static VideoMode[] ReadVideoModeBitmask(uint rawVal)
    {
        var modes = new List<VideoMode>();

        // Check each possible VideoMode enum value
        foreach (var possibleMode in Enum.GetValues<VideoMode>())
        {
            if ((rawVal & (1u << (int)possibleMode)) != 0)
            {
                modes.Add(possibleMode);
            }
        }

        return modes.ToArray();
    }
}
