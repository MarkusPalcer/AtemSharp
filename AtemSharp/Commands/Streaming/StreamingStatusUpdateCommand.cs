using System.Diagnostics.CodeAnalysis;
using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.State.Streaming;

namespace AtemSharp.Commands.Streaming;

[Command("StRS", ProtocolVersion.V8_1_1)]
internal class StreamingStatusUpdateCommand : IDeserializedCommand
{
    [ExcludeFromCodeCoverage]
    public StreamingStatus Status { get; internal set; }

    [ExcludeFromCodeCoverage]
    public StreamingError Error { get; internal set; }


    // Manually deserialized command because of one bitfield setting two properties
    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        var rawStatus = rawCommand.ReadUInt16BigEndian(0);

        var decodedStatus = Enum.GetValues<StreamingStatus>()
                                .Cast<ushort>()
                                .Reverse()
                                .Where(status => (rawStatus & status) > 0)
                                .Select(status => (StreamingStatus)status)
                                .First();

        var decodedError = Enum.GetValues<StreamingError>()
                               .Cast<ushort>()
                               .Where(error => (rawStatus & error) > 0)
                               .Select(error => (StreamingError)error)
                               .FirstOrDefault();

        return new StreamingStatusUpdateCommand
        {
            Error = decodedError,
            Status = decodedStatus,
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Streaming.Error = Error;
        state.Streaming.Status = Status;
    }
}
