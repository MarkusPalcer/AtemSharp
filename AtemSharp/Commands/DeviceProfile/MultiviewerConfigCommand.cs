using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_MvC")]
public class MultiviewerConfigCommand : IDeserializedCommand
{
    public int Count { get; init; }
    public int WindowCount { get; init; }

    // Store the protocol version for use in ApplyToState
    public ProtocolVersion Version { get; init; }

    public static MultiviewerConfigCommand Deserialize(ReadOnlySpan<byte> buffer, ProtocolVersion version)
    {
        var result = new MultiviewerConfigCommand
        {
            Count = version >= ProtocolVersion.V8_1_1 ? 0 : buffer.ReadUInt8(0),
            WindowCount = version >= ProtocolVersion.V8_1_1 ? buffer.ReadUInt8(0) : buffer.ReadUInt8(1),
            Version = version
        };

        return result;
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // For V8_1_1+, Count is not part of the protocol, so only update WindowCount
        if (Version >= ProtocolVersion.V8_1_1)
        {
            // Only update WindowCount, preserve Count
            state.Info.MultiViewer.WindowCount = WindowCount;
        }
        else
        {
            // For older versions, update both properties
            state.Info.MultiViewer.Count = Count;
            state.Info.MultiViewer.WindowCount = WindowCount;
        }
    }
}
