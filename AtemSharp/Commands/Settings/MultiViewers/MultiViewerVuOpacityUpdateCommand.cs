using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

// TODO: Split into two commands to be consistent with other commands
/// <summary>
/// Command to set MultiViewer VU opacity level
/// </summary>
[Command("VuMo")]
[BufferSize(4)]
public partial class MultiViewerVuOpacityUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _multiViewerId;

    /// <summary>
    /// VU opacity level (0-100)
    /// </summary>
    [DeserializedField(1)]
    private byte _opacity;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Validate state prerequisites (same pattern as TypeScript update commands)
        if (state.Info.MultiViewer.Count == 0 || _multiViewerId >= state.Info.MultiViewer.Count)
        {
            throw new InvalidIdError("MultiViewer", _multiViewerId);
        }

        // Get or create the MultiViewer and update its VU opacity
        var multiViewer = AtemStateUtil.GetMultiViewer(state, _multiViewerId);
        multiViewer.VuOpacity = Opacity;
    }
}
