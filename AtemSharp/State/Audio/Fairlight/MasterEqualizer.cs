namespace AtemSharp.State.Audio.Fairlight;

public class MasterEqualizer : Equalizer
{
    public MasterEqualizerBand[] Bands { get; internal set; } = [];
}