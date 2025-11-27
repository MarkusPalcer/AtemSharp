using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_MvC", ProtocolVersion.V8_1_1)]
public partial class MultiviewerConfigCommandV811 : IDeserializedCommand
{
    [DeserializedField(0)] private byte _windowCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Info.MultiViewer.WindowCount = WindowCount;
    }
}
