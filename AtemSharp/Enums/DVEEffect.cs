namespace AtemSharp.Enums;

/// <summary>
/// DVE transition effect types
/// </summary>
// ReSharper disable once InconsistentNaming Domain Specific Acronym
public enum DVEEffect : byte
{
    SwooshTopLeft = 0,
    SwooshTop = 1,
    SwooshTopRight = 2,
    SwooshLeft = 3,
    SwooshRight = 4,
    SwooshBottomLeft = 5,
    SwooshBottom = 6,
    SwooshBottomRight = 7,
    SpinCWTopLeft = 8,
    SpinCWTopRight = 9,
    SpinCWBottomLeft = 10,
    SpinCWBottomRight = 11,
    SpinCCWTopLeft = 12,
    SpinCCWTopRight = 13,
    SpinCCWBottomLeft = 14,
    SpinCCWBottomRight = 15
}