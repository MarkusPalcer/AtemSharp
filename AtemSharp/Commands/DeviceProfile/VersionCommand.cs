using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Version command received from ATEM device
/// </summary>
[Command("_ver")]
public class VersionCommand : IDeserializedCommand
{
    /// <summary>
    /// ATEM protocol version
    /// </summary>
    public ProtocolVersion Version { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <returns>Deserialized command instance</returns>
    public static VersionCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new VersionCommand
        {
            Version = (ProtocolVersion)rawCommand.ReadUInt32BigEndian(0)
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update the API version in device info
        state.Info.ApiVersion = Version;
    }
}
