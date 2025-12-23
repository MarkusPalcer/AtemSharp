namespace AtemSharp.State.Audio.Fairlight;

/// <summary>
/// The shape of the equalizer band
/// </summary>
/// <remarks>
/// Naming according to the "Fairlight Audio Guide to DaVinci Resolve" by BlackMagic Design
/// </remarks>
public enum Shape : byte
{
    LowShelf = 1,
    LowPass = 2,
    BellCurve = 4,
    Notch = 8,
    HighPass = 16,
    HighShelf = 32,
}
