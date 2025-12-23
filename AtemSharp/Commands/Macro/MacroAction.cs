namespace AtemSharp.Commands.Macro;

/// <summary>
/// An action to execute on a macro
/// </summary>
public enum MacroAction : byte
{
    /// <summary>
    /// Run the macro
    /// </summary>
    Run = 0,

    /// <summary>
    /// Stop the macro if running
    /// </summary>
    Stop = 1,

    /// <summary>
    /// Stop recording this macro
    /// </summary>
    StopRecord = 2,

    /// <summary>
    /// Insert a pause in the currently recording macro
    /// </summary>
    InsertUserWait = 3,

    /// <summary>
    /// Continue a stopped macro
    /// </summary>
    Continue = 4,

    /// <summary>
    /// Delete the macro
    /// </summary>
    Delete = 5,
}
