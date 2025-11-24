namespace AtemSharp.State.Audio.Fairlight;

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