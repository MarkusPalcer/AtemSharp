namespace AtemSharp.State.Audio.Fairlight;

/// <summary>
/// Fairlight audio input state
/// </summary>
public class FairlightAudioInput
{
    public ushort Id { get; internal set; }
    public FairlightAudioInputProperties Properties { get; internal set; } = new();

    public Dictionary<long, Source> Sources { get; } = [];
}

public abstract class Equalizer
{
    public bool Enabled { get; internal set; }
    public double Gain { get; internal set; }
}

public class SourceEqualizer : Equalizer
{
    public SourceEqualizerBand[] Bands { get; internal set; } = [];
}

public class MasterEqualizer : Equalizer
{
    public MasterEqualizerBand[] Bands { get; internal set; } = [];
}

public class Dynamics
{
    public double MakeUpGain { get; internal set; }

    public Expander Expander { get;  } = new();
    public Compressor Compressor { get;  } = new();

    public Limiter Limiter { get; } = new();
}

public class Limiter
{
    public bool Enabled { get; internal set; }
    public double Threshold { get; internal set; }
    public double Attack { get; internal set; }
    public double Hold { get; internal set; }
    public double Release { get; internal set; }
}

public class Compressor
{
    public bool Enabled { get; internal set; }
    public double Threshold { get; internal set; }
    public double Ratio { get; internal set; }
    public double Attack { get; internal set; }
    public double Hold { get; internal set; }
    public double Release { get; internal set; }
}

public class Expander
{
    public bool Enabled { get; internal set; }
    public bool GateEnabled { get; internal set; }
    public double Threshold { get; internal set; }
    public double Range { get; internal set; }
    public double Ratio { get; internal set; }
    public double Attack { get; internal set; }
    public double Hold { get; internal set; }
    public double Release { get; internal set; }
}

public class SourceEqualizerBand : EqualizerBand
{
    public long SourceId { get; internal set; }

    public ushort InputId { get; internal set; }
}

public class MasterEqualizerBand : EqualizerBand;

public abstract class EqualizerBand
{
    public byte Id { get; internal set; }

    public bool Enabled { get; internal set; }

    // TODO: Create enum from ATEM Mini ISO Pro behavior
    public byte[] SupportedShapes { get; internal set; } = [];
    public byte Shape { get; internal set; }

    // TODO: Create enum from ATEM Mini ISO Pro behavior
    public byte[] SupportedFrequencyRanges { get; internal set; } = [];
    public byte FrequencyRange { get; internal set; }
    public uint Frequency { get; internal set; }
    public double Gain { get; internal set; }
    public double QFactor { get; internal set; }
}
