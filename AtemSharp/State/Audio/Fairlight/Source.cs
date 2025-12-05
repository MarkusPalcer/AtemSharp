using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class Source
{
    public long Id { get; internal set; }

    public SourceEqualizer Equalizer { get; } = new();

    public Dynamics Dynamics { get; } = new();
    public FairlightAudioSourceType Type { get; internal set; }
    public byte MaxFramesDelay { get; internal set; }
    public byte FramesDelay { get; internal set; }
    public double Gain { get; internal set; }
    public bool HasStereoSimulation { get; internal set; }
    public double StereoSimulation { get; internal set; }
    public double Balance { get; internal set; }
    public double FaderGain { get; internal set; }
    public FairlightAudioMixOption[] SupportedMixOptions { get; internal set; } = [];
    public FairlightAudioMixOption MixOption { get; internal set; }
    public ushort InputId { get; internal set; }
}
