using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.State.Recording;

namespace AtemSharp.Commands.Recording;

[Command("RTMD", ProtocolVersion.V8_1_1)]
internal partial class RecordingDiskInfoUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private uint _diskId;

    [CustomDeserialization] private bool _isDelete;

    [DeserializedField(8)] private RecordingDiskStatus _status;

    [DeserializedField(4)] private uint _recordingTimeAvailable;

    // Stryker disable once string : initialization is always overriden by deserialization
    [CustomDeserialization] private string _name = string.Empty;

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand)
    {
        _isDelete = _status.HasFlag(RecordingDiskStatus.Removed);
        _status &= ~RecordingDiskStatus.Removed;

        _status &= RecordingDiskStatus.All;

        _name = rawCommand.ReadString(10, 64);
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        if (IsDelete)
        {
            state.Recording.Disks.Remove(DiskId);
        }
        else
        {
            var disk = state.Recording.Disks.GetOrCreate(DiskId);
            disk.Name = Name;
            disk.Status = Status;
            disk.RecordingTimeAvailable = RecordingTimeAvailable;
        }
    }
}
