namespace AtemSharp.Enums;

/// <summary>
/// Indicates which key frame positions are set for fly key functionality
/// </summary>
[Flags]
public enum IsAtKeyFrame : byte
{
    /// <summary>
    /// No key frame set
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Key frame A is set
    /// </summary>
    A = 1 << 0,
    
    /// <summary>
    /// Key frame B is set
    /// </summary>
    B = 1 << 1,
    
    /// <summary>
    /// Run to infinite is set
    /// </summary>
    RunToInfinite = 1 << 2,
}