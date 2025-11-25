using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Command to set MultiViewer properties (layout and program/preview swap)
/// </summary>
[Command("CMvP", ProtocolVersion.V8_0)]
[BufferSize(4)]
public partial class MultiViewerPropertiesCommand(MultiViewer multiViewer) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _multiViewerId = multiViewer.Id;

    /// <summary>
    /// MultiViewer layout configuration
    /// </summary>
    [SerializedField(2, 0)] private MultiViewerLayout _layout = multiViewer.Properties.Layout;

    /// <summary>
    /// Whether program and preview outputs are swapped
    /// </summary>
    [SerializedField(3, 1)] private bool _programPreviewSwapped = multiViewer.Properties.ProgramPreviewSwapped;
}
