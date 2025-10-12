namespace AtemSharp.Enums;

/// <summary>
/// Layout options for MultiViewer output
/// </summary>
public enum MultiViewerLayout : byte
{
    /// <summary>
    /// Default layout
    /// </summary>
    Default = 0,
    
    /// <summary>
    /// Top left window is small
    /// </summary>
    TopLeftSmall = 1,
    
    /// <summary>
    /// Top right window is small
    /// </summary>
    TopRightSmall = 2,
    
    /// <summary>
    /// Program output shown at bottom (combination of TopLeftSmall and TopRightSmall)
    /// </summary>
    ProgramBottom = TopLeftSmall | TopRightSmall,
    
    /// <summary>
    /// Bottom left window is small
    /// </summary>
    BottomLeftSmall = 4,
    
    /// <summary>
    /// Program output shown at right (combination of TopLeftSmall and BottomLeftSmall)
    /// </summary>
    ProgramRight = TopLeftSmall | BottomLeftSmall,
    
    /// <summary>
    /// Bottom right window is small
    /// </summary>
    BottomRightSmall = 8,
    
    /// <summary>
    /// Program output shown at left (combination of TopRightSmall and BottomRightSmall)
    /// </summary>
    ProgramLeft = TopRightSmall | BottomRightSmall,
    
    /// <summary>
    /// Program output shown at top (combination of BottomLeftSmall and BottomRightSmall)
    /// </summary>
    ProgramTop = BottomLeftSmall | BottomRightSmall,
}