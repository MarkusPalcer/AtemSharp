using AtemSharp.Enums;
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
    public int MultiViewerId { get; set; }

    /// <summary>
    /// MultiViewer layout configuration
    /// </summary>
    public MultiViewerLayout Layout { get; set; }

    /// <summary>
    /// Whether program and preview outputs are swapped
    /// </summary>
    public bool ProgramPreviewSwapped { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static MultiViewerPropertiesUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var multiViewerId = reader.ReadByte();
        var layout = (MultiViewerLayout)reader.ReadByte();
        var programPreviewSwapped = reader.ReadByte() > 0;

        return new MultiViewerPropertiesUpdateCommand
        {
            MultiViewerId = multiViewerId,
            Layout = layout,
            ProgramPreviewSwapped = programPreviewSwapped
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
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

        // Return the state path that was modified for change tracking
        return [$"settings.multiViewers.{MultiViewerId}.properties"];
    }
}