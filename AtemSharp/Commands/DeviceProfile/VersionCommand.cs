using System.Text;
using AtemSharp.Enums;
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
    public ProtocolVersion Version { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static VersionCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        // Read 32-bit big-endian protocol version
        var version = (ProtocolVersion)reader.ReadUInt32BigEndian();

        return new VersionCommand
        {
            Version = version
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
    {
        // Update the API version in device info
        state.Info.ApiVersion = Version;

        // Return the state path that was modified
        return ["info.apiVersion"];
    }
}