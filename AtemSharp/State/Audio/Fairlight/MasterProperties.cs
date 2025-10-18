namespace AtemSharp.State.Audio.Fairlight;

public class MasterProperties
{
    /// <summary>
    /// Whether audio follows video during crossfade transition
    /// </summary>
    public bool AudioFollowsVideo { get; set; }

    public double FaderGain { get; set; }

    public Dynamics Dynamics { get;  } = new();
    public MasterEqualizer Equalizer { get;  } = new();
    public bool FollowFadeToBlack { get; set; }
}
