using AtemSharp.Enums;
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
    public int MultiViewerId { get; set; }

    /// <summary>
    /// The window index within the MultiViewer
    /// </summary>
    public int WindowIndex { get; set; }

    /// <summary>
    /// Whether VU meter display is enabled for this window
    /// </summary>
    public bool VuEnabled { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static MultiViewerWindowVuMeterUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var multiViewerId = reader.ReadByte();
        var windowIndex = reader.ReadByte();
        var vuEnabled = reader.ReadBoolean();

        return new MultiViewerWindowVuMeterUpdateCommand
        {
            MultiViewerId = multiViewerId,
            WindowIndex = windowIndex,
            VuEnabled = vuEnabled
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

        // Return the state path that was modified for change tracking
        return new[] { $"settings.multiViewers.{MultiViewerId}.windows.{WindowIndex}.audioMeter" };
    }
}