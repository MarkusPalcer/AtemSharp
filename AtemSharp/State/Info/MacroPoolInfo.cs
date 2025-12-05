using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Info;

/// <summary>
/// Macro pool configuration and capabilities
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MacroPoolInfo
{
    /// <summary>
    /// Number of macros available in the macro pool
    /// </summary>
    public byte MacroCount { get; internal set; }
}
