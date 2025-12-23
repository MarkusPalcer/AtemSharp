using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.State.Recording;

namespace AtemSharp.Commands.Recording;

/// <summary>
/// Used to enable/disable recording
/// </summary>
/// <param name="state"></param>
[Command("RcTM", ProtocolVersion.V8_1_1)]
[BufferSize(4)]
public partial class RecordingStatusCommand(AtemState state) : SerializedCommand
{
    [SerializedField(0)] private bool _isRecording = state.Recording.Status == RecordingStatus.Recording;
}
