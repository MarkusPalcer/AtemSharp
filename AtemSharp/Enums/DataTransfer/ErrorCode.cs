namespace AtemSharp.Enums.DataTransfer;

/// <summary>
/// Error codes that can be returned by data transfer operations
/// </summary>
public enum ErrorCode : byte
{
    /// <summary>
    /// The operation should be retried
    /// </summary>
    Retry = 1,

    /// <summary>
    /// The requested resource was not found
    /// </summary>
    NotFound = 2,

    /// <summary>
    /// The resource is not locked (maybe)
    /// </summary>
    NotLocked = 5
}