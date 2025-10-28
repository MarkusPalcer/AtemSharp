namespace AtemSharp.Enums;

/// <summary>
/// Mix effect availability flags
/// </summary>
[Flags]
public enum MeAvailability : byte
{
    None = 0,
    Me1 = 1 << 0,
    Me2 = 1 << 1,
    Me3 = 1 << 2,
    Me4 = 1 << 3,
    All = Me1 | Me2 | Me3 | Me4,
}
