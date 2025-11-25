namespace AtemSharp.State.Audio.Fairlight;

public class SourceEqualizer : Equalizer
{
    public SourceEqualizerBand[] Bands { get; internal set; } = [];
}
