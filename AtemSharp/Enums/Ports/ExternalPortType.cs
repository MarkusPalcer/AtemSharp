namespace AtemSharp.Enums.Ports;

/// <summary>
/// External port types
/// </summary>
[Flags]
public enum ExternalPortType : ushort
{
    Unknown = 0,
    SDI = 1,
    HDMI = 2,
    Component = 4,
    Composite = 8,
    SVideo = 16,
    XLR = 32,
    AESEBU = 64,
    RCA = 128,
    Internal = 256,
    TSJack = 512,
    MADI = 1024,
    TRSJack = 2048,
    RJ45 = 4096,
}
