using AtemSharp.Enums;
using AtemSharp.Lib;
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
    public int SuperSourceId { get; init; }

    /// <summary>
    /// Number of SuperSource boxes available
    /// </summary>
    public byte BoxCount { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static SuperSourceConfigCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        if (protocolVersion >= ProtocolVersion.V8_0)
        {
            return new SuperSourceConfigCommand
            {
                SuperSourceId = rawCommand.ReadUInt8(0),
                BoxCount = rawCommand.ReadUInt8(2)
            };
        }
        else
        {
            return new SuperSourceConfigCommand
            {
                SuperSourceId = 0,
                BoxCount = rawCommand.ReadUInt8(0)
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
        state.Info.SuperSources[SuperSourceId] = new SuperSourceInfo
        {
            BoxCount = BoxCount
        };
    }
}
