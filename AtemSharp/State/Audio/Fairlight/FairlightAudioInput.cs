using AtemSharp.Enums.Fairlight;

namespace AtemSharp.State.Audio.Fairlight;

/// <summary>
/// Fairlight audio input state
/// </summary>
public class FairlightAudioInput
{
    public ushort Id { get; internal set; }
    public FairlightAudioInputProperties Properties { get; set; } = new();

    public Dictionary<long, Source> Sources { get; } = [];
}

public class Source
{
    public long Id { get; internal set; }

    public Equalizer Equalizer { get; } = new();

    public Dynamics Dynamics { get; } = new();
    public FairlightAudioSourceType Type { get; set; }
    public byte MaxFramesDelay { get; set; }
    public byte FramesDelay { get; set; }
    public double Gain { get; set; }
    public bool HasStereoSimulation { get; set; }
    public double StereoSimulation { get; set; }
    public double Balance { get; set; }
    public double FaderGain { get; set; }
    public FairlightAudioMixOption[] SupportedMixOptions { get; set; }
    public FairlightAudioMixOption MixOption { get; set; }
    public ushort InputId { get; set; }
}

public class Equalizer
{
    public bool Enabled { get; set; }
    public double Gain { get; set; }

    public Band[] Bands { get; internal set; } = [];
}

public class Dynamics
{
    public double MakeUpGain { get; set; }
}

public class Band
{
}
