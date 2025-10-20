using AtemSharp.Enums.Fairlight;

namespace AtemSharp.State.Audio.Fairlight;

public class Source
{
    public long Id { get; internal set; }

    public SourceEqualizer Equalizer { get; } = new();

    public Dynamics Dynamics { get; } = new();
    public FairlightAudioSourceType Type { get; set; }
    public byte MaxFramesDelay { get; set; }
    public byte FramesDelay { get; set; }
    public double Gain { get; set; }
    public bool HasStereoSimulation { get; set; }
    public double StereoSimulation { get; set; }
    public double Balance { get; set; }
    public double FaderGain { get; set; }
    public FairlightAudioMixOption[] SupportedMixOptions { get; set; } = [];
    public FairlightAudioMixOption MixOption { get; set; }
    public ushort InputId { get; set; }
}
