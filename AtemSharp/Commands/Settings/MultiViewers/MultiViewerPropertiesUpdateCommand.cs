using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.State.Settings.MultiViewer;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Command received from ATEM device containing MultiViewer properties update
/// </summary>
[Command("MvPr", ProtocolVersion.V8_0)]
public partial class MultiViewerPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _multiViewerId;

    /// <summary>
    /// MultiViewer layout configuration
    /// </summary>
    [DeserializedField(1)] private MultiViewerLayout _layout;

    /// <summary>
    /// Whether program and preview outputs are swapped
    /// </summary>
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
