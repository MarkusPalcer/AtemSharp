using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Media;

[Command("CMPS", ProtocolVersion.V8_0)]
public class MediaPoolSettingsSetCommand(MediaPoolSettings settings) : SerializedCommand
{
    public ushort[] MaxFrames { get; } = settings.MaxFrames.ToArray();

    // Custom serialization because array size must not be changed
    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[8];
        buffer.WriteUInt16BigEndian(MaxFrames[0], 0);
        buffer.WriteUInt16BigEndian(MaxFrames[1], 2);
        buffer.WriteUInt16BigEndian(MaxFrames[2], 4);
        buffer.WriteUInt16BigEndian(MaxFrames[3], 6);

        return buffer;
    }
}
