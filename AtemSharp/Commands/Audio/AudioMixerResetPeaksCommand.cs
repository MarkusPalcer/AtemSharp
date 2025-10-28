using AtemSharp.Helpers;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to reset audio mixer peak indicators
/// </summary>
[Command("RAMP")]
[BufferSize(8)]
public partial class AudioMixerResetPeaksCommand : SerializedCommand
{
    /// <summary>
        /// Reset all audio input peaks
        /// </summary>
    [SerializedField(1,0)]
    private bool _all;

    /// <summary>
        /// Audio input index to reset peaks for (0-based)
        /// </summary>
    [SerializedField(2,1)]
    private ushort _input;

    /// <summary>
    /// Reset master audio peaks
    /// </summary>
    [SerializedField(4,2)]
    private bool _master;

    /// <summary>
        /// Reset monitor audio peaks
        /// </summary>
    [SerializedField(5,3)]
    private bool _monitor;

    /// <summary>
    /// Create command with all reset options disabled
    /// </summary>
    public AudioMixerResetPeaksCommand()
    {
        // Initialize with all options false (no flags set)
        _all = false;
        _input = 0;
        _master = false;
        _monitor = false;
    }
}
