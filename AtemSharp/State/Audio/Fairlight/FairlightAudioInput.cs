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

public abstract class Equalizer
{
    public bool Enabled { get; set; }
    public double Gain { get; set; }
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

public class SourceEqualizerBand : EqualizerBand
{
    public long SourceId { get; internal set; }

    public ushort InputId { get; internal set; }
}

public class MasterEqualizerBand : EqualizerBand;

public abstract class EqualizerBand
{
    public byte Id { get; internal set; }

    public bool Enabled { get; set; }

    // TODO: Create enum from ATEM Mini ISO Pro behavior
    public byte[] SupportedShapes { get; set; } = [];
    public byte Shape { get; set; }

    // TODO: Create enum from ATEM Mini ISO Pro behavior
    public byte[] SupportedFrequencyRanges { get; set; } = [];
    public byte FrequencyRange { get; set; }
    public uint Frequency { get; set; }
    public double Gain { get; set; }
    public double QFactor { get; set; }
}
