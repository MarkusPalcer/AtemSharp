namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Command to request the current display clock time
/// </summary>
[Command("DSTR")]
[BufferSize(4)]
public partial class DisplayClockRequestTimeCommand : SerializedCommand
{
    // Field will exist in the future according to TS implementation
    // For now we use it to trigger code generation ;)
    [SerializedField(0)]
    [NoProperty]
    private byte _id = 0;

    /// <summary>
    /// Create command to request display clock time
    /// </summary>
    public DisplayClockRequestTimeCommand()
    {
    }
}
