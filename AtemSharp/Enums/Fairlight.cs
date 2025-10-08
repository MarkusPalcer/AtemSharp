namespace AtemSharp.Enums;

/// <summary>
/// Fairlight audio mix options
/// </summary>
public enum FairlightAudioMixOption
{
    Off = 1,
    On = 2,
    AudioFollowVideo = 4,
}

/// <summary>
/// Fairlight input configuration
/// </summary>
public enum FairlightInputConfiguration
{
    Mono = 1,
    Stereo = 2,
    DualMono = 4,
}

/// <summary>
/// Fairlight analog input level
/// </summary>
public enum FairlightAnalogInputLevel
{
    Microphone = 1,
    ConsumerLine = 2,
    // [Since(ProtocolVersion.V8_1_1)]
    ProLine = 4,
}

/// <summary>
/// Fairlight audio source type
/// </summary>
public enum FairlightAudioSourceType
{
    Mono = 0,
    Stereo = 1,
}

/// <summary>
/// Fairlight input type
/// </summary>
public enum FairlightInputType
{
    EmbeddedWithVideo = 0,
    MediaPlayer = 1,
    AudioIn = 2,
    MADI = 4,
}