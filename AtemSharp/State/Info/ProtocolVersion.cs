namespace AtemSharp.State.Info;

/// <summary>
/// ATEM protocol versions
/// </summary>
public enum ProtocolVersion
{
    Unknown = 0x00000000,
    V7_2 = 0x00020016, // 2.22 // TODO - verify this is correct
    V7_5_2 = 0x0002001b, // 2.27 // The naming of this may be off
    V8_0 = 0x0002001c, // 2.28
    V8_0_1 = 0x0002001d, // 2.29
    V8_1_1 = 0x0002001e, // 2.30
    V9_4 = 0x0002001f, // 2.31
    V9_6 = 0x00020020, // 2.32
}
