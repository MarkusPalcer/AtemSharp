using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public class Source
{
    public Source()
    {
        Equalizer = new SourceEqualizer(this);
    }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public long Id { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public SourceEqualizer Equalizer { get; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public Dynamics Dynamics { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public FairlightAudioSourceType Type { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public byte MaxFramesDelay { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public byte FramesDelay { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public double Gain { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public bool HasStereoSimulation { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public double StereoSimulation { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public double Balance { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public double FaderGain { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public FairlightAudioMixOption[] SupportedMixOptions { get; internal set; } = [];

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public FairlightAudioMixOption MixOption { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ushort InputId { get; internal set; }

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{GetType().Name} #{InputId}.#{Id}";
}
