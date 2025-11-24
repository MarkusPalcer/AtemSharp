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