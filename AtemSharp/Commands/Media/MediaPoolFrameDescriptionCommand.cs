using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Media;

[Command("MPfe")]
public class MediaPoolFrameDescriptionCommand : IDeserializedCommand
{
    public byte MediaPoolId { get; init; }
    public ushort FrameIndex { get; init; }
    public bool IsUsed { get; init; }
    public required string Hash { get; init; }
    public required string FileName { get; init; }

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> data, ProtocolVersion version) =>
        new MediaPoolFrameDescriptionCommand
        {
            MediaPoolId = data.ReadUInt8(0),
            FrameIndex = data.ReadUInt16BigEndian(2),
            IsUsed = data.ReadBoolean(4),
            Hash = Convert.ToBase64String(data.Slice(5,16)),
            FileName = data.ReadString(24, data.ReadUInt8(23))
        };

    public void ApplyToState(AtemState state)
    {
        MediaPoolEntry entry;

        switch (MediaPoolId)
        {
            case 0:
                entry = state.Media.Frames[FrameIndex];
            break;
            case 3:
                entry = state.Media.Clips[FrameIndex];
                break;
            default:
                throw new InvalidOperationException("Unknown MediaPoolId: " + MediaPoolId);
        }

        entry.FileName = FileName;
        entry.IsUsed = IsUsed;
        entry.Hash = Hash;
    }
}
