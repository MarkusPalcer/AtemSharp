using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Command received from ATEM device containing MultiViewer window VU meter update
/// </summary>
[Command("VuMC")]
public class MultiViewerWindowVuMeterUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// MultiViewer ID for this update
    /// </summary>
    public int MultiViewerId { get; init; }

    /// <summary>
    /// The window index within the MultiViewer
    /// </summary>
    public int WindowIndex { get; init; }

    /// <summary>
    /// Whether VU meter display is enabled for this window
    /// </summary>
    public bool VuEnabled { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static MultiViewerWindowVuMeterUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new MultiViewerWindowVuMeterUpdateCommand
        {
            MultiViewerId = rawCommand.ReadUInt8(0),
            WindowIndex = rawCommand.ReadUInt8(1),
            VuEnabled = rawCommand.ReadBoolean(2)
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

        // Get the MultiViewer and update its window
        var multiViewer = AtemStateUtil.GetMultiViewer(state, MultiViewerId);

        // Get the current window state or create a new one
        if (!multiViewer.Windows.TryGetValue(WindowIndex, out var currentWindow))
        {
            currentWindow = new MultiViewerWindowState();
            multiViewer.Windows[WindowIndex] = currentWindow;
        }

        // Update the VU meter state
        currentWindow.AudioMeter = VuEnabled;
    }
}
