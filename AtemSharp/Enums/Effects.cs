namespace AtemSharp.Enums;

/// <summary>
/// Border bevel types
/// </summary>
public enum BorderBevel
{
    None = 0,
    InOut = 1,
    In = 2,
    Out = 3,
}

/// <summary>
/// Keyframe indicators
/// </summary>
[Flags]
public enum IsAtKeyFrame
{
    None = 0,
    A = 1 << 0,
    B = 1 << 1,
    RunToInfinite = 1 << 2,
}

/// <summary>
/// Wipe patterns
/// </summary>
public enum Pattern
{
    LeftToRightBar = 0,
    TopToBottomBar = 1,
    HorizontalBarnDoor = 2,
    VerticalBarnDoor = 3,
    CornersInFourBox = 4,
    RectangleIris = 5,
    DiamondIris = 6,
    CircleIris = 7,
    TopLeftBox = 8,
    TopRightBox = 9,
    BottomRightBox = 10,
    BottomLeftBox = 11,
    TopCentreBox = 12,
    RightCentreBox = 13,
    BottomCentreBox = 14,
    LeftCentreBox = 15,
    TopLeftDiagonal = 16,
    TopRightDiagonal = 17,
}

/// <summary>
/// Mix Effect key types
/// </summary>
public enum MixEffectKeyType
{
    Luma = 0,
    Chroma = 1,
    Pattern = 2,
    DVE = 3,
}

/// <summary>
/// Fly key keyframes
/// </summary>
public enum FlyKeyKeyFrame
{
    None = 0,
    A = 1,
    B = 2,
    Full = 3,
    RunToInfinite = 4,
}

/// <summary>
/// Fly key direction
/// </summary>
public enum FlyKeyDirection
{
    CentreOfKey = 0,
    TopLeft = 1,
    TopCentre = 2,
    TopRight = 3,
    MiddleLeft = 4,
    MiddleCentre = 5,
    MiddleRight = 6,
    BottomLeft = 7,
    BottomCentre = 8,
    BottomRight = 9,
}

/// <summary>
/// SuperSource art options
/// </summary>
public enum SuperSourceArtOption
{
    Background,
    Foreground,
}