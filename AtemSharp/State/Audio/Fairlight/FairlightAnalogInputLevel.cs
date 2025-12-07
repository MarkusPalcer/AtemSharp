namespace AtemSharp.State.Audio.Fairlight;

public enum FairlightAnalogInputLevel : byte {
    /// <summary>
    /// Represents that the analog input level is not supported by the device
    /// </summary>
    None = 0,
    Microphone = 1,
    ConsumerLine = 2,
    ProLine = 4,
}
