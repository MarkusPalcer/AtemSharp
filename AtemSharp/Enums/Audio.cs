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

/// <summary>
/// Audio channel pairs
/// </summary>
public enum AudioChannelPair
{
    Channel1_2 = 0,
    Channel3_4 = 1,
    Channel5_6 = 2,
    Channel7_8 = 3,
    Channel9_10 = 4,
    Channel11_12 = 5,
    Channel13_14 = 6,
    Channel15_16 = 7,
}

/// <summary>
/// Audio internal port types
/// </summary>
public enum AudioInternalPortType
{
    NotInternal = 0, // TODO - verify
    NoAudio = 1, // TODO - verify
    TalkbackMix = 2, // TODO - verify
    EngineeringTalkbackMix = 3, // TODO - verify
    ProductionTalkbackMix = 4, // TODO - verify
    MediaPlayer = 5, // TODO - verify
    Program = 6,
    Return = 7,
    Monitor = 8, // TODO - verify
    Madi = 9,
    AuxOut = 10,
    AudioAuxOut = 11, // TODO - verify
}