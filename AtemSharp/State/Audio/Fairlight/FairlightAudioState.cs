using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Audio.Fairlight;

/// <summary>
/// Fairlight audio state for ATEM devices with Fairlight audio support
/// </summary>
public class FairlightAudioState : AudioState
{
    public FairlightAudioState()
    {
        Inputs = new ItemCollection<ushort, FairlightAudioInput>(id => new FairlightAudioInput { Id = id });
    }

    /// <summary>
    /// Fairlight audio inputs indexed by input number
    /// </summary>
    /// <remarks>
    /// Fairlight input IDs are not array indices but have semantics
    /// </remarks>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<ushort, FairlightAudioInput> Inputs { get; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public MasterProperties Master { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public MonitorProperties Monitor { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public SoloProperties Solo { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public AudioRouting AudioRouting { get; } = new();
}
