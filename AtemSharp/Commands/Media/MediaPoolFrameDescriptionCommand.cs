using AtemSharp.Attributes;
using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Media;

[Command("MPfe")]
public partial class MediaPoolFrameDescriptionCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _mediaPoolId;

    [DeserializedField(2)]
    private ushort _frameIndex;

    [DeserializedField(4)]
    private bool _isUsed;

    [DeserializedField(5)]
    [CustomDeserialization]
    private string _hash = string.Empty;

    [DeserializedField(24)]
    [CustomDeserialization]
    private string _fileName = string.Empty;

    private void DeserializeInternal(ReadOnlySpan<byte> data, ProtocolVersion _)
    {
        Hash = Convert.ToBase64String(data.Slice(5, 16));
        FileName = data.ReadString(24, data.ReadUInt8(23));
    }

    public void ApplyToState(AtemState state)
    {
        var entry = MediaPoolId switch
        {
            0 => state.Media.Frames[FrameIndex],
            3 => state.Media.Clips[FrameIndex],
            _ => throw new InvalidOperationException("Unknown MediaPoolId: " + MediaPoolId)
        };

        entry.FileName = FileName;
        entry.IsUsed = IsUsed;
        entry.Hash = Hash;
    }
}
