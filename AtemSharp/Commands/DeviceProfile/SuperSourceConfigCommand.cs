using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// SuperSource configuration command received from ATEM
/// </summary>
/// <remarks>
/// Use for protocol versions before 8.0
/// </remarks>
[Command("_SSC")]
public partial class SuperSourceConfigCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _boxCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Info.SuperSources[0].BoxCount = BoxCount;
    }
}
