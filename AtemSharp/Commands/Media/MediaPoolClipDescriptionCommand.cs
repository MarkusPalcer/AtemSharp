using AtemSharp.State;

namespace AtemSharp.Commands.Media;

[Command("MPCS")]
public partial class MediaPoolClipDescriptionCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _clipId;
    [DeserializedField(1)] private bool _isUsed;
    [CustomDeserialization] private string _name = string.Empty;
    [DeserializedField(66)] private ushort _frameCount;

    private void DeserializeInternal(ReadOnlySpan<byte> data)
    {
        _name = data.ReadString(2, 64);
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var clip = state.Media.Clips[_clipId];
        clip.Name = _name;
        clip.IsUsed = _isUsed;
        clip.FrameCount = _frameCount;
    }
}
