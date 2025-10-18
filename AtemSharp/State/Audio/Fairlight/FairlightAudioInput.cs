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
    public FairlightAudioMixOption[] SupportedMixOptions { get; set; } = [];
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

    public Expander Expander { get;  } = new();
    public Compressor Compressor { get;  } = new();

    public Limiter Limiter { get; } = new();
}

public class Limiter
{
    public bool Enabled { get; set; }
    public double Threshold { get; set; }
    public double Attack { get; set; }
    public double Hold { get; set; }
    public double Release { get; set; }
}

public class Compressor
{
    public bool Enabled { get; set; }
    public double Threshold { get; set; }
    public double Ratio { get; set; }
    public double Attack { get; set; }
    public double Hold { get; set; }
    public double Release { get; set; }
}

public class Expander
{
    public bool Enabled { get; set; }
    public bool GateEnabled { get; set; }
    public double Threshold { get; set; }
    public double Range { get; set; }
    public double Ratio { get; set; }
    public double Attack { get; set; }
    public double Hold { get; set; }
    public double Release { get; set; }
}

public class Band
{
    public byte Index { get; internal set; }

    public ushort InputId { get; internal set; }
    public long SourceId { get; internal set; }

    public bool Enabled { get; set; }
    public uint[] SupportedShapes { get; set; } = [];
    public byte Shape { get; set; }
    public uint[] SupportedFrequencyRanges { get; set; } = [];
    public byte FrequencyRange { get; set; }
    public uint Frequency { get; set; }
    public double Gain { get; set; }
    public double QFactor { get; set; }
}
