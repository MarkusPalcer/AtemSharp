namespace AtemSharp.Enums;

/// <summary>
/// Media source types
/// </summary>
public enum MediaSourceType
{
    Still = 1,
    Clip,
}

/// <summary>
/// Audio mix options
/// </summary>
public enum AudioMixOption : byte
{
    Off = 0,
    On = 1,
    AudioFollowVideo = 2,
}

/// <summary>
/// Audio source types
/// </summary>
public enum AudioSourceType
{
    ExternalVideo,
    MediaPlayer,
    ExternalAudio,
}