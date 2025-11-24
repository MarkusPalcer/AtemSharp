namespace AtemSharp.State.Audio.Fairlight;

public class SourceEqualizerBand : EqualizerBand
{
    public long SourceId { get; internal set; }

    public ushort InputId { get; internal set; }
}