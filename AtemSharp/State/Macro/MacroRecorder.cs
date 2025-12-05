using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Macro;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MacroRecorder
{
    public bool IsRecording { get; internal set; }
    public ushort MacroIndex { get; internal set; }
}
