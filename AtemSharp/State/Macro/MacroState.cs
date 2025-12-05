using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Macro;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MacroState
{
    public MacroPlayer Player { get; } = new();

    public MacroRecorder Recorder { get; } = new();

    public Macro[] Macros { get; internal set; } = [];
}
