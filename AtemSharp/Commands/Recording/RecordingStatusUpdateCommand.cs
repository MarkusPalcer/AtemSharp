using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Recording;

namespace AtemSharp.Commands.Recording;

[Command("RTMS", ProtocolVersion.V8_1_1)]
public class RecordingStatusUpdateCommand : IDeserializedCommand
{
    public uint? RecordingTimeAvailable { get; internal set; }

    public RecordingStatus Status { get; internal set; }

    public RecordingError Error { get; internal set; }

    // Manually deserialized command because of one bitfield setting two properties
    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        var rawStatus = rawCommand.ReadUInt16BigEndian(0);

        var decodedStatus = Enum.GetValues<RecordingStatus>()
                                .Cast<ushort>()
                                .Where(status => (rawStatus & status) > 0)
                                .Select(status => (RecordingStatus)status).FirstOrDefault();

        var decodedError = Enum.GetValues<RecordingError>()
                               .Cast<ushort>()
                               .Reverse()
                               .Where(error => (rawStatus & error) > 0)
                               .Select(error => (RecordingError)error).FirstOrDefault();

        return new RecordingStatusUpdateCommand
        {
            RecordingTimeAvailable = rawCommand.Length > 4 ? rawCommand.ReadUInt32BigEndian(4) : null,
            Status = decodedStatus,
            Error = decodedError
        };
    }

    public void ApplyToState(AtemState state)
    {
        state.Recording.Status = Status;
        state.Recording.Error = Error;
        state.Recording.RecordingTimeAvailable = RecordingTimeAvailable;
    }
}
