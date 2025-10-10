namespace AtemSharp.Enums;

/// <summary>
/// ATEM device models
/// </summary>
public enum Model : byte
{
    Unknown = 0x00,
    TVS = 0x01,
    OneME = 0x02,
    TwoME = 0x03,
    PS4K = 0x04,
    OneME4K = 0x05,
    TwoME4K = 0x06,
    TwoMEBS4K = 0x07,
    TVSHD = 0x08,
    TVSProHD = 0x09,
    TVSPro4K = 0x0a,
    Constellation = 0x0b,
    Constellation8K = 0x0c,
    Mini = 0x0d,
    MiniPro = 0x0e,
    MiniProISO = 0x0f,
    MiniExtreme = 0x10,
    MiniExtremeISO = 0x11,
    ConstellationHD1ME = 0x12,
    ConstellationHD2ME = 0x13,
    ConstellationHD4ME = 0x14,
    SDI = 0x15,
    SDIProISO = 0x16,
    SDIExtremeISO = 0x17,
    // 0x18 ??
    // 0x19 ??
    TelevisionStudioHD8 = 0x1a,
    TelevisionStudioHD8ISO = 0x1b,
    Constellation4K1ME = 0x1c,
    Constellation4K2ME = 0x1d,
    Constellation4K4ME = 0x1e,
    // 0x1f ??
    TelevisionStudio4K8 = 0x20,
}