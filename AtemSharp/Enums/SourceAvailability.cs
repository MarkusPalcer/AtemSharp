namespace AtemSharp.Enums;

/// <summary>
/// Source availability flags
/// </summary>
[Flags]
public enum SourceAvailability
{
    None = 0,
    Auxiliary = 1 << 0,
    Multiviewer = 1 << 1,
    SuperSourceArt = 1 << 2,
    SuperSourceBox = 1 << 3,
    KeySource = 1 << 4,
    Auxiliary1 = 1 << 5,
    Auxiliary2 = 1 << 6,
    All = Auxiliary | Multiviewer | SuperSourceArt | SuperSourceBox | KeySource | Auxiliary1 | Auxiliary2,
}