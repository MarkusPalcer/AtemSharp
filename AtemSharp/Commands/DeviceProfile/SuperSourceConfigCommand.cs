using System.Text;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// SuperSource configuration command received from ATEM
/// </summary>
[Command("_SSC")]
public class SuperSourceConfigCommand : IDeserializedCommand
{
    /// <summary>
    /// SuperSource ID (0-based index)
    /// </summary>
    public int SsrcId { get; set; }

    /// <summary>
    /// Number of SuperSource boxes available
    /// </summary>
    public byte BoxCount { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static SuperSourceConfigCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        if (protocolVersion >= ProtocolVersion.V8_0)
        {
            var ssrcId = reader.ReadByte();
            reader.ReadByte(); // Skip padding byte
            var boxCount = reader.ReadByte();

            return new SuperSourceConfigCommand
            {
                SsrcId = ssrcId,
                BoxCount = boxCount
            };
        }
        else
        {
            var boxCount = reader.ReadByte();

            return new SuperSourceConfigCommand
            {
                SsrcId = 0,
                BoxCount = boxCount
            };
        }
    }

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Paths indicating what was changed in the state</returns>
    public void ApplyToState(AtemState state)
    {
        // Update device info SuperSource configuration
        state.Info.SuperSources[SsrcId] = new SuperSourceInfo
        {
            BoxCount = BoxCount
        };
    }
}
