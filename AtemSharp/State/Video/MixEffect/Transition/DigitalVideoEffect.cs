namespace AtemSharp.State.Video.MixEffect.Transition;

/// <summary>
/// DVE transition effect types
/// </summary>
// ReSharper disable once InconsistentNaming Domain Specific Acronym
public enum DigitalVideoEffect : byte
{
    SwooshTopLeft = 0,
    SwooshTop = 1,
    SwooshTopRight = 2,
    SwooshLeft = 3,
    SwooshRight = 4,
    SwooshBottomLeft = 5,
    SwooshBottom = 6,
    SwooshBottomRight = 7,
    SpinClockWiseTopLeft = 8,
    SpinClockWiseTopRight = 9,
    SpinClockWiseBottomLeft = 10,
    SpinClockWiseBottomRight = 11,
    SpinCounterClockWiseTopLeft = 12,
    SpinCounterClockWiseTopRight = 13,
    SpinCounterClockWiseBottomLeft = 14,
    SpinCounterClockWiseBottomRight = 15
}
