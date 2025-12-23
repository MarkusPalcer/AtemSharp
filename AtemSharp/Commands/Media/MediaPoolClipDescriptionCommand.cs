using AtemSharp.State;

namespace AtemSharp.Commands.Media;

[Command("MPCS")]
internal partial class MediaPoolClipDescriptionCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _clipId;
    [DeserializedField(1)] private bool _isUsed;
    // Stryker disable once string : initialization is always overriden by deserialization
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
