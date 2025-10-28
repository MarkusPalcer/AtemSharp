using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Recording;

[Command("CRMS", ProtocolVersion.V8_1_1)]
[BufferSize(144)]
public partial class RecordingSettingsCommand(AtemState state) : SerializedCommand
{
    [SerializedField(1, 0)]
    [CustomSerialization]
    private string _fileName = state.Recording.FileName;

    [SerializedField(132, 1)]
    private uint _workingSet1DiskId = state.Recording.WorkingSet1DiskId;

    [SerializedField(136,2)]
    private uint _workingSet2DiskId = state.Recording.WorkingSet2DiskId;

    [SerializedField(140,3)]
    private bool _recordInAllCameras = state.Recording.RecordInAllCameras;

    private void SerializeInternal(byte[] rawCommand)
    {
        rawCommand.WriteString(_fileName, 1, 128);
    }
}
