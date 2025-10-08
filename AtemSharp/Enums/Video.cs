namespace AtemSharp.Enums;

/// <summary>
/// Video formats
/// </summary>
public enum VideoFormat
{
    SD,
    HD720,
    HD1080,
    UHD4K,
    UDH8K,
}

/// <summary>
/// Video modes
/// </summary>
public enum VideoMode
{
    N525i5994NTSC = 0,
    P625i50PAL = 1,
    N525i5994169 = 2,
    P625i50169 = 3,

    P720p50 = 4,
    N720p5994 = 5,
    P1080i50 = 6,
    N1080i5994 = 7,
    N1080p2398 = 8,
    N1080p24 = 9,
    P1080p25 = 10,
    N1080p2997 = 11,
    P1080p50 = 12,
    N1080p5994 = 13,

    N4KHDp2398 = 14,
    N4KHDp24 = 15,
    P4KHDp25 = 16,
    N4KHDp2997 = 17,

    P4KHDp5000 = 18,
    N4KHDp5994 = 19,

    N8KHDp2398 = 20,
    N8KHDp24 = 21,
    P8KHDp25 = 22,
    N8KHDp2997 = 23,
    P8KHDp50 = 24,
    N8KHDp5994 = 25,

    N1080p30 = 26,
    N1080p60 = 27,
}