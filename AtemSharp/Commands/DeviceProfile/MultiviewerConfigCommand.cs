using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_MvC")]
public partial class MultiviewerConfigCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _count;
    [DeserializedField(1)] private byte _windowCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Info.MultiViewer.Count = Count;
        state.Info.MultiViewer.WindowCount = WindowCount;
    }
}
