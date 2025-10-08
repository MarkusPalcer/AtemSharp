namespace AtemSharp.Enums;

/// <summary>
/// ATEM protocol versions
/// </summary>
public enum ProtocolVersion
{
    V7_2 = 0x00020016, // 2.22 // TODO - verify this is correct
    V7_5_2 = 0x0002001b, // 2.27 // The naming of this may be off
    V8_0 = 0x0002001c, // 2.28
    V8_0_1 = 0x0002001d, // 2.29
    V8_1_1 = 0x0002001e, // 2.30
    V9_4 = 0x0002001f, // 2.31
    V9_6 = 0x00020020, // 2.32
}

/// <summary>
/// Transition styles
/// </summary>
public enum TransitionStyle
{
    MIX = 0x00,
    DIP = 0x01,
    WIPE = 0x02,
    DVE = 0x03,
    STING = 0x04,
}

/// <summary>
/// Transition selection flags
/// </summary>
[Flags]
public enum TransitionSelection
{
    Background = 1 << 0,
    Key1 = 1 << 1,
    Key2 = 1 << 2,
    Key3 = 1 << 3,
    Key4 = 1 << 4,
}