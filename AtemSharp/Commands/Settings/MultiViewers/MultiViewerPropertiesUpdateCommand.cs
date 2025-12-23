using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.State.Settings.MultiViewer;

namespace AtemSharp.Commands.Settings.MultiViewers;

[Command("MvPr", ProtocolVersion.V8_0)]
internal partial class MultiViewerPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _multiViewerId;
    [DeserializedField(1)] private MultiViewerLayout _layout;
    [DeserializedField(2)] private bool _programPreviewSwapped;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var multiViewer = state.Settings.MultiViewers.GetOrCreate(MultiViewerId);
        multiViewer.Id = _multiViewerId;
        multiViewer.Properties.Layout = Layout;
        multiViewer.Properties.ProgramPreviewSwapped = ProgramPreviewSwapped;
    }
}
