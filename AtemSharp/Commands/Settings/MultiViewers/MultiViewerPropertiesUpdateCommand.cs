using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Command received from ATEM device containing MultiViewer properties update
/// </summary>
[Command("MvPr", ProtocolVersion.V8_0)]
public partial class MultiViewerPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _multiViewerId;

    /// <summary>
    /// MultiViewer layout configuration
    /// </summary>
    [DeserializedField(1)]
    private MultiViewerLayout _layout;

    /// <summary>
    /// Whether program and preview outputs are swapped
    /// </summary>
    [DeserializedField(2)]
    private bool _programPreviewSwapped;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Validate state prerequisites (same pattern as TypeScript update commands)
        if (state.Info.MultiViewer.Count == 0 || MultiViewerId >= state.Info.MultiViewer.Count)
        {
            throw new InvalidIdError("MultiViewer", MultiViewerId);
        }

        // Get or create the MultiViewer and update its properties
        var multiViewer = AtemStateUtil.GetMultiViewer(state, MultiViewerId);
        multiViewer.Properties.Layout = Layout;
        multiViewer.Properties.ProgramPreviewSwapped = ProgramPreviewSwapped;
    }
}
