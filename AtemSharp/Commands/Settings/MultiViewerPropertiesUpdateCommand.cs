using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Command received from ATEM device containing MultiViewer properties update
/// </summary>
[Command("MvPr", ProtocolVersion.V8_0)]
public class MultiViewerPropertiesUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// MultiViewer ID for this update
    /// </summary>
    public int MultiViewerId { get; init; }

    /// <summary>
    /// MultiViewer layout configuration
    /// </summary>
    public MultiViewerLayout Layout { get; init; }

    /// <summary>
    /// Whether program and preview outputs are swapped
    /// </summary>
    public bool ProgramPreviewSwapped { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static MultiViewerPropertiesUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new MultiViewerPropertiesUpdateCommand
        {
            MultiViewerId = rawCommand.ReadUInt8(0),
            Layout = (MultiViewerLayout)rawCommand.ReadUInt8(1),
            ProgramPreviewSwapped = rawCommand.ReadBoolean(2)
        };
    }

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
        multiViewer.Properties = new MultiViewerPropertiesState
        {
            Layout = Layout,
            ProgramPreviewSwapped = ProgramPreviewSwapped
        };
    }
}
