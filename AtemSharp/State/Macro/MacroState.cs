using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Macro;

public class MacroState
{
    internal MacroState()
    {
        Macros = new ItemCollection<ushort, Macro>(id => new Macro { Id = id }, 1);
    }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public MacroPlayer Player { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public MacroRecorder Recorder { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<ushort, Macro> Macros { get; }
}
