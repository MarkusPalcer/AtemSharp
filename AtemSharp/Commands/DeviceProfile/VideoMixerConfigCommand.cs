using AtemSharp.State.Info;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_VMC")]
// ReSharper disable once RedundantExtendsListEntry Needed for CodeGen
internal partial class VideoMixerConfigCommand : VideoMixerConfigCommandBase, IDeserializedCommand
{
    [DeserializedField(0)] private ushort _videoModeCount;

    private void DeserializeInternal(ReadOnlySpan<byte> data)
    {
        SupportedVideoModes = new SupportedVideoMode[_videoModeCount];
        for (var i = 0; i < _videoModeCount; i++)
        {
            var videoModeData = data.Slice(4 + i * 12, 12);
            SupportedVideoModes[i] = ParseVideoMode(videoModeData, ProtocolVersion.V7_2);
        }
    }
  }
