using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

public class BandUpdateParameters
{
    public byte BandIndex { get; set; }

    public bool Enabled { get; set; }

    // TODO: Create enum from ATEM Mini ISO Pro behavior
    public uint[] SupportedShapes { get; set; } = [];

    public byte Shape { get; set; }

    public uint[] SupportedFrequencyRanges { get; set; } = [];

    // TODO: Create enum from ATEM Mini ISO Pro behavior
    public byte FrequencyRange { get; set; }

    public uint Frequency { get; set; }

    public double Gain { get; set; }

    public double QFactor { get; set; }

    public void ApplyTo(EqualizerBand band)
    {
        band.Enabled = Enabled;
        band.SupportedShapes = SupportedShapes;
        band.Shape = Shape;
        band.SupportedFrequencyRanges = SupportedFrequencyRanges;
        band.FrequencyRange = FrequencyRange;
        band.Frequency = Frequency;
        band.Gain = Gain;
        band.QFactor = QFactor;
    }
}
