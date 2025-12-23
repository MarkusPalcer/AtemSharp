using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Recording;

[Command("RMSu", ProtocolVersion.V8_1_1)]
internal partial class RecordingSettingsUpdateCommand : IDeserializedCommand
{
    // Stryker disable once string : initialization is always overriden by deserialization
    [CustomDeserialization] private string _fileName = string.Empty;

    [DeserializedField(128)] private uint _workingSet1DiskId;

    [DeserializedField(132)] private uint _workingSet2DiskId;

    [DeserializedField(136)] private bool _recordInAllCameras;

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand)
    {
        _fileName = rawCommand.ReadString(0, 128);
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Recording.FileName = _fileName;
        state.Recording.WorkingSet1DiskId = _workingSet1DiskId;
        state.Recording.WorkingSet2DiskId = _workingSet2DiskId;
        state.Recording.RecordInAllCameras = _recordInAllCameras;
    }
}
