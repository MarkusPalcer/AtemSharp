using AtemSharp.State.Info;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Command to receive video mixer configuration information
/// </summary>
/// <remarks>
/// Used starting protocol version 8.0
/// </remarks>
[Command("_VMC", ProtocolVersion.V8_0)]
// ReSharper disable once RedundantExtendsListEntry Needed for CodeGen
public partial class VideoMixerConfigCommandV8 : VideoMixerConfigCommandBase, IDeserializedCommand
{
    [DeserializedField(0)] private ushort _videoModeCount;

    private void DeserializeInternal(ReadOnlySpan<byte> data)
    {
        SupportedVideoModes = new SupportedVideoMode[_videoModeCount];
        for (var i = 0; i < _videoModeCount; i++)
        {
            var videoModeData = data.Slice(4 + i * 13, 13);
            SupportedVideoModes[i] = ParseVideoMode(videoModeData, ProtocolVersion.V8_0);
        }
    }
}
