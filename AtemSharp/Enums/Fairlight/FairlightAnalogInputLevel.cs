namespace AtemSharp.Enums.Fairlight;

public enum FairlightAnalogInputLevel : byte {
    Microphone = 1,
    ConsumerLine = 2,
    // [Since(ProtocolVersion.V8_1_1)]
    ProLine = 4,
}
