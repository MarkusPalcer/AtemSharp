using AtemSharp.Commands;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Version command properties
/// </summary>
public record VersionProperties(ProtocolVersion Version);

/// <summary>
/// Version command received from ATEM containing protocol version
/// </summary>
public class VersionCommand : DeserializedCommand<VersionProperties>
{
    public static new string RawName { get; } = "_ver";

    public VersionCommand(ProtocolVersion version) : base(new VersionProperties(version))
    {
    }

    public static VersionCommand Deserialize(byte[] rawCommand)
    {
        var version = (ProtocolVersion)ReadUInt32BE(rawCommand, 0);
        return new VersionCommand(version);
    }

    public override string[] ApplyToState(AtemState state)
    {
        state.Info.ApiVersion = Properties.Version;
        return new[] { "info.apiVersion" };
    }

    private static uint ReadUInt32BE(byte[] buffer, int offset)
    {
        return ((uint)buffer[offset] << 24) |
               ((uint)buffer[offset + 1] << 16) |
               ((uint)buffer[offset + 2] << 8) |
               buffer[offset + 3];
    }
}

/// <summary>
/// Initialization complete command received from ATEM
/// </summary>
public class InitCompleteCommand : DeserializedCommand<object?>
{
    public static new string RawName { get; } = "InCm";

    public InitCompleteCommand() : base(null)
    {
    }

    public static InitCompleteCommand Deserialize(byte[] rawCommand)
    {
        return new InitCompleteCommand();
    }

    public override string[] ApplyToState(AtemState state)
    {
        return new[] { "info" };
    }
}